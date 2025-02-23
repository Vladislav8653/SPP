using System.Text;
using Faker.Contracts;
using Faker.Parameters;

namespace Faker.TypeGenerators;

public class StringGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        byte[] buffer = new byte[10];
        context.Random.NextBytes(buffer);
        return Encoding.UTF8.GetString(buffer);
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(string);
    }
}