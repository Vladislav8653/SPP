using Faker.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class ArrayGenerator : IValueGenerator
{
    private const int MaxArrayLength = 7;
    private const int MinArrayLength = 3;
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetElementType();
        var arrayLength = context.Random.Next(MinArrayLength, MaxArrayLength);
        var array = Array.CreateInstance(type, arrayLength);
        for (int i = 0; i < arrayLength; i++)
        {
           array.SetValue(context.Faker.Create(type), i);
        }

        return array;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsArray;
    }
}