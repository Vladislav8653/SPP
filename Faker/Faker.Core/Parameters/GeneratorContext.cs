using Faker.Core.Implementation;

namespace Faker.Core.Parameters;

public class GeneratorContext
{
    public Random Random { get; }

    public Implementation.Faker Faker { get; }

    public GeneratorContext(Random random, Implementation.Faker faker)
    {
        Random = random;
        Faker = faker;
    }
}