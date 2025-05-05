using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestsGenerator.Library;

public class TestGenerator
{
    private readonly int _maxFileRead;
    private readonly int _maxTestGeneration;
    private readonly int _maxFileWrite;

    public TestGenerator(int maxFileRead, int maxTestGeneration, int maxFileWrite)
    {
        _maxFileRead = maxFileRead;
        _maxTestGeneration = maxTestGeneration;
        _maxFileWrite = maxFileWrite;
    }
    
    public TestGenerator() : this(Environment.ProcessorCount, Environment.ProcessorCount, Environment.ProcessorCount)
    { }

    public Task GenerateTestsAsync(IEnumerable<string> inputFiles, string outputFolder)
    {
        Directory.CreateDirectory(outputFolder);
        
        var readBlock = new TransformBlock<string, FileContent>(async filePath => 
        {
            string content = await File.ReadAllTextAsync(filePath);
            return new FileContent { FilePath = filePath, Content = content };
        }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxFileRead });


        var generateBlock = new TransformManyBlock<FileContent, TestFile>(
            input => GenerateTestFiles(input.Content), 
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxTestGeneration });

        
        var writeBlock = new ActionBlock<TestFile>(async testFile =>
            {
                await File.WriteAllTextAsync(Path.Combine(outputFolder, testFile.FileName), testFile.Content);
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxFileWrite });

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        readBlock.LinkTo(generateBlock, linkOptions);
        generateBlock.LinkTo(writeBlock, linkOptions);

        // Запускаем конвейер – отправляем имена файлов в первый блок.
        foreach (var файлик in inputFiles)
        {
            readBlock.Post(файлик);
        }
        readBlock.Complete();

        return writeBlock.Completion;
    }
    
    private IEnumerable<TestFile> GenerateTestFiles(string sourceCode)
    {
        // where Roslyn is used (CSharpSyntaxTree is their lib).
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = syntaxTree.GetRoot();

        // public classes
        var classes = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(cls => cls.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));

        foreach (var classDecl in classes)
        {
            var namespaceDecl = classDecl.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>();
            
            // ???
            string namespaceName = namespaceDecl != null ? namespaceDecl.Name.ToString() : "GlobalNamespace";

            string testClassName = classDecl.Identifier.Text + "Tests";
            
            string testClassContent = $$"""
                                        using System;
                                        using Xunit;
                                        using {{namespaceName}};

                                        namespace {{namespaceName}}.Tests
                                        {
                                            public class {{testClassName}}
                                            {
                                        {{string.Join(Environment.NewLine, GetClassMethods(classDecl))}}
                                            }
                                        }
                                        """;

            yield return new TestFile
            {
                FileName = testClassName + ".cs",
                Content = testClassContent
            };
        }
    }

    private List<string> GetClassMethods(ClassDeclarationSyntax @class)
    {
        // Находим все публичные методы класса.
        var methods = @class.Members.OfType<MethodDeclarationSyntax>()
            .Where(m => m.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)))
            .ToList();
        
        var methodCounts = new Dictionary<string, int>();
        foreach (var methodName in methods
                     .Select(method => method.Identifier.Text)
                     .Where(methodName => !methodCounts.TryAdd(methodName, 1)))
        {
            methodCounts[methodName]++;
        }

        // Для методов с перегрузками задаём порядковый номер.
        var overloadIndices = new Dictionary<string, int>();
        var testMethods = new List<string>();
        foreach (var method in methods)
        {
            string methodName = method.Identifier.Text;
            int count = methodCounts[methodName];
            int index = 0;
            if (count > 1)
            {
                if (!overloadIndices.TryAdd(methodName, 1))
                    overloadIndices[methodName]++;
                index = overloadIndices[methodName];
            }
            
            string testMethodName = count > 1 ? $"{methodName}{index}Test" : $"{methodName}Test";
            
            string testMethodCode = $$"""
                                      
                                              [Fact]
                                              public void {{testMethodName}}()
                                              {
                                                  Assert.True(false, "autogenerated");
                                              }
                                      """;
            testMethods.Add(testMethodCode);
        }

        return testMethods;
    }
}