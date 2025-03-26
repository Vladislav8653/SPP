using System.Collections;
using Faker.Core.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class TypeIEnumerableGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First(); // тип дженерика
        // MakeGenericType возвращает тип, подставляя type как параметр дженерика
        //typeof (List<>) получает тип List<> без типа дженерика
        return (IEnumerable)context.Faker.Create(typeof(List<>).MakeGenericType(type));
        
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }
}