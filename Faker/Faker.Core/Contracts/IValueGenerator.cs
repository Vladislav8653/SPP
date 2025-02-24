using Faker.Core.Parameters;

namespace Faker.Contracts;

public interface IValueGenerator
{
    object Generate(Type typeToGenerate, GeneratorContext context);
   
    bool CanGenerate(Type type);
}

