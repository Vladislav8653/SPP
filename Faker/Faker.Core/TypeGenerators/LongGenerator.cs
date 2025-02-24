using Faker.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class LongGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextInt64();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(long);
    }
}