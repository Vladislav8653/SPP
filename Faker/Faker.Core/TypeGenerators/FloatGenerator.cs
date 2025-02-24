using Faker.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class FloatGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextSingle();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(float);
    }
}