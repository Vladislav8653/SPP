namespace Faker.Tests;

[TestFixture]
public class Tests
{
    private Core.Implementation.Faker _faker;

    [SetUp]
    public void Setup()
    {
        _faker = new Core.Implementation.Faker(null);
    }

    [Test]
    public void ArrayGenerator_ShouldGenerateArray()
    {
        var result = _faker.Create<int[]>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<int[]>(result);
    }

    [Test]
    public void DoubleGenerator_ShouldGenerateDouble()
    {
        var result = _faker.Create<double>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<double>(result);
    }

    [Test]
    public void FloatGenerator_ShouldGenerateFloat()
    {
        var result = _faker.Create<float>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<float>(result);
    }

    [Test]
    public void GuidGenerator_ShouldGenerateGuid()
    {
        var result = _faker.Create<Guid>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<Guid>(result);
    }

    [Test]
    public void IntGenerator_ShouldGenerateInt()
    {
        var result = _faker.Create<int>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<int>(result);
    }

    [Test]
    public void ListGenerator_ShouldGenerateList()
    {
        var result = _faker.Create<List<int>>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<List<int>>(result);
    }

    [Test]
    public void LongGenerator_ShouldGenerateLong()
    {
        var result = _faker.Create<long>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<long>(result);
    }

    [Test]
    public void StringGenerator_ShouldGenerateString()
    {
        var result = _faker.Create<string>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<string>(result);
    }

    [Test]
    public void TypeICollectionGenerator_ShouldGenerateICollection()
    {
        var result = _faker.Create<ICollection<int>>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ICollection<int>>(result);
    }

    [Test]
    public void TypeIEnumerableGenerator_ShouldGenerateIEnumerable()
    {
        var result = _faker.Create<IEnumerable<int>>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<IEnumerable<int>>(result);
    }

    [Test]
    public void TypeIListGenerator_ShouldGenerateIList()
    {
        var result = _faker.Create<IList<int>>();
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<IList<int>>(result);
    }
}