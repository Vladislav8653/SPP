using System.Collections;
using Faker.Contracts;
using Faker.Core.Parameters;

namespace Faker.TypeGenerators;

public class TypeIListGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First(); // тип дженерика
        // MakeGenericType возвращает тип, подставляя type как параметр дженерика
        //typeof (List<>) получает тип List<> без типа дженерика
        return (IList)context.Faker.Create(typeof(List<>).MakeGenericType(type));
        
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
    }
}