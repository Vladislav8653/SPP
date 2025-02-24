using Faker.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class GuidGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return new Guid();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(Guid);
    }
}