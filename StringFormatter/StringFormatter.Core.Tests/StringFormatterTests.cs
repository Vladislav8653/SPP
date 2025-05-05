namespace StringFormatter.Core.Tests;

public class StringFormatterTests
{
    private readonly StringFormatter _formatter = new();

    private class TestClass
    {
    }

    [Fact]
    public void Format_Replaces_SimpleProperties()
    {
        var obj = new TestClass();
        var template = "Name: {Name}, Age: {Age}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("Name: Bob, Age: 42", result);
    }

    [Fact]
    public void Format_Handles_NestedProperties()
    {
        var obj = new TestClass();
        var template = "Nested: {Nested.Info}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("Nested: Deep", result);
    }

    [Fact]
    public void Format_Handles_ArrayIndexing()
    {
        var obj = new TestClass();
        var template = "First: {Numbers[0]}, Second: {Numbers[1]}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("First: 10, Second: 20", result);
    }

    [Fact]
    public void Format_Handles_ListIndexing()
    {
        var obj = new TestClass();
        var template = "Word: {Words[1]}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("Word: bar", result);
    }

    [Fact]
    public void Format_Handles_MultidimensionalIndexing()
    {
        var obj = new TestClass();
        var template = "Matrix[1][0]: {Matrix[1][0]}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("Matrix[1][0]: 3", result);
    }

    [Fact]
    public void Format_Returns_EmptyString_ForNullProperty()
    {
        var obj = new TestClass();
        var template = "Null: {NullProp}";
        var result = _formatter.Format(template, obj);

        Assert.Equal("Null: ", result);
    }

    [Fact]
    public void Format_Throws_For_FieldAccess()
    {
        var obj = new TestClass();
        var template = "Field: {FieldValue}";

        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }

    [Fact]
    public void Format_Throws_For_InvalidIndexing()
    {
        var obj = new TestClass();
        var template = "Invalid: {Age[0]}";

        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }

    [Fact]
    public void Format_Throws_For_PropertyNotFound()
    {
        var obj = new TestClass();
        var template = "Nope: {DoesNotExist}";

        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }

    [Fact]
    public void Format_Throws_For_UnbalancedBraces()
    {
        var obj = new TestClass();
        var template1 = "Oops {Name";
        var template2 = "Oops }Name{";

        Assert.Throws<FormatException>(() => _formatter.Format(template1, obj));
        Assert.Throws<FormatException>(() => _formatter.Format(template2, obj));
    }

    [Fact]
    public void Format_Throws_If_TargetIsNull()
    {
        var template = "{Name}";
        Assert.Throws<ArgumentNullException>(() => _formatter.Format(template, null!));
    }

    [Fact]
    public void Format_Throws_If_TemplateIsNull()
    {
        var obj = new TestClass();
        Assert.Throws<ArgumentNullException>(() => _formatter.Format(null!, obj));
    }
}
