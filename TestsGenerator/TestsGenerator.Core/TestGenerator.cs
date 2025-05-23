using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestsGenerator.Core;

public class TestGenerator(int readLimit, int testGenLimit, int writeLimit)
{
    public TestGenerator() : this(Environment.ProcessorCount, 
        Environment.ProcessorCount, Environment.ProcessorCount) { }

    public Task GenerateTestsAsync(IEnumerable<string> inputFiles, string outputFolder)
    {
        Directory.CreateDirectory(outputFolder);
        
        var readBlock = new TransformBlock<string, FileContent>(async filePath => 
        {
            var content = await File.ReadAllTextAsync(filePath);
            return new FileContent { FilePath = filePath, Content = content };
        }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = readLimit });


        var generateBlock = new TransformManyBlock<FileContent, TestFile>(
            input => GenerateTestFiles(input.Content), 
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = testGenLimit });

        
        var writeBlock = new ActionBlock<TestFile>(async testFile =>
            {
                await File.WriteAllTextAsync(Path.Combine(outputFolder, testFile.FileName), testFile.Content);
            },
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = writeLimit });

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        readBlock.LinkTo(generateBlock, linkOptions);
        generateBlock.LinkTo(writeBlock, linkOptions);
        
        foreach (var файлик in inputFiles)
        {
            readBlock.Post(файлик);
        }
        readBlock.Complete();

        return writeBlock.Completion;
    }
    
    private IEnumerable<TestFile> GenerateTestFiles(string sourceCode) =>
        CSharpSyntaxTree.ParseText(sourceCode)
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(IsPublicClass)
            .Select(BuildTestFile);

    private TestFile BuildTestFile(ClassDeclarationSyntax classDecl)
    {
        var namespaceName = GetNamespace(classDecl) ?? "GlobalNamespace";
        var testClassName = $"{classDecl.Identifier.Text}Tests";
        
        return new TestFile
        {
            FileName = $"{testClassName}.cs",
            Content =
                $@"using System;
                using Xunit;
                using {namespaceName};

                namespace {namespaceName}.Tests
                {{
                    public class {testClassName}
                    {{
                        {string.Join(Environment.NewLine, GenerateTestMethods(classDecl).Select(line => "        " + line))}
                    }}
                }}"
        };

    }

    private static List<string> GenerateTestMethods(ClassDeclarationSyntax classSyntax)
    {
        return classSyntax.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(IsPublicMethod)
            .GroupBy(m => m.Identifier.Text)
            .SelectMany(g => g.Select((method, index) => 
                GenerateTestMethod(method.Identifier.Text, g.Count(), index + 1)))
            .ToList();
    }

    private static string GenerateTestMethod(string methodName, int overloadCount, int overloadIndex)
    {
        var testMethodName = overloadCount > 1 
            ? $"{methodName}{overloadIndex}Test" 
            : $"{methodName}Test";

        return $$"""
            
                    [Fact]
                    public void {{testMethodName}}()
                    {
                        Assert.True(false, "autogenerated method should not be called");
                    }
            """;
    }

    private static bool IsPublicClass(ClassDeclarationSyntax cls) 
        => cls.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));

    private static bool IsPublicMethod(MethodDeclarationSyntax method) 
        => method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));

    private static string? GetNamespace(SyntaxNode node) 
        => node.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>()?.Name.ToString();
}