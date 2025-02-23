using Faker.Contracts;
using Faker.Parameters;

namespace Faker.TypeGenerators;

public class DoubleGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextDouble();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(double);
    }
}