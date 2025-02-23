using System.Collections;
using Faker.Contracts;
using Faker.Parameters;

namespace Faker.TypeGenerators;

public class ListGenerator : IValueGenerator
{
    private const int MaxListLength = 7;
    private const int MinListLength = 3;
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First(); // тип дженерика
        var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;
        var listCount = context.Random.Next(MinListLength, MaxListLength);
        for (int i = 0; i < listCount; i++)
        {
            list?.Add(context.Faker.Create(type));
        }
        
        return list!;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }
}