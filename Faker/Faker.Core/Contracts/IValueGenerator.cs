using Faker.Core.Parameters;

namespace Faker.Core.Contracts;

public interface IValueGenerator
{
    object Generate(Type typeToGenerate, GeneratorContext context);
   
    bool CanGenerate(Type type);
}

