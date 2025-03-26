using Faker.Core.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class IntGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.Next();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(int);
    }
}