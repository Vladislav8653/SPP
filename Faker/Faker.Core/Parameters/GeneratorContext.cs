using Faker.Contracts;

namespace Faker.Parameters;

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