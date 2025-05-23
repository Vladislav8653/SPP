using TestsGenerator.Core;

namespace TestsGenerator.Tests;

public class TestGeneratorTests
{
    [Fact]
    public async Task GenerateTestsAsync_CreatesTestFiles_ForPublicClasses_CustomSample()
    {
        var tempInputDir = Path.Combine(Path.GetTempPath(), "GenTest_Input");
        var tempOutputDir = Path.Combine(Path.GetTempPath(), "GenTest_Output");
        Directory.CreateDirectory(tempInputDir);
        Directory.CreateDirectory(tempOutputDir);

        var sampleCode = """
                         using System;

                         namespace DemoProject
                         {
                             public class Calculator
                             {
                                 public int Add(int x, int y) { return x + y; }
                                 public int Subtract(int x, int y) { return x - y; }
                                 public int Subtract(int x, int y, int z) { return x - y - z; }
                             }
                         }
                         """;
        var inputFile = Path.Combine(tempInputDir, "DemoInput.cs");
        await File.WriteAllTextAsync(inputFile, sampleCode);

        var generator = new TestGenerator();
        await generator.GenerateTestsAsync([inputFile], tempOutputDir);

        var expectedTestFile = Path.Combine(tempOutputDir, "CalculatorTests.cs");
        Assert.True(File.Exists(expectedTestFile), "Test file was not created.");

        var content = await File.ReadAllTextAsync(expectedTestFile);

        Assert.Contains("[Fact]", content);
        Assert.Contains("public void AddTest()", content);
        Assert.Contains("public void Subtract1Test()", content);
        Assert.Contains("public void Subtract2Test()", content);
        Assert.Contains("""Assert.True(false, "autogenerated method should not be called");""", content);

        Directory.Delete(tempInputDir, true);
        Directory.Delete(tempOutputDir, true);
    }

}