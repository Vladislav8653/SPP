using System.Reflection;
using Faker.Contracts;
using Faker.Core.Parameters;
using Faker.TypeGenerators;

namespace Faker.Core.Implementation;

public class Faker(FakerConfig? config)
{
    private readonly List<IValueGenerator> _generators =
    [
        new ArrayGenerator(),
        new DoubleGenerator(),
        new FloatGenerator(),
        new GuidGenerator(),
        new IntGenerator(),
        new ListGenerator(),
        new LongGenerator(),
        new StringGenerator(),
        new TypeICollectionGenerator(),
        new TypeIEnumerableGenerator(),
        new TypeIListGenerator(),
    ];

    private readonly Random _random = new Random();

    public T Create<T>()
    {
        return (T)Create(typeof(T));
    }

    public object Create(Type t)
    {
        var generator = _generators.FirstOrDefault(g => g.CanGenerate(t));
        if (generator == null)
        {
            return CreateCompositeType(t);
        }

        return generator.Generate(t, new GeneratorContext(_random, this));
    }

    private object CreateCompositeType(Type type)
    {
        var constructors = type.GetConstructors().OrderBy(c => c.GetParameters().Length).ToList();
        foreach (var constructor in constructors)
        {
            try
            {
                var ctrParamsInfo = constructor.GetParameters();
                var ctrParams = ctrParamsInfo.Select(p => GenerateConstructorParameter(type, p)).ToArray();
                var instance = constructor.Invoke(ctrParams);
                InitializePublicMembers(instance, ctrParamsInfo);
                return instance;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return GetDefaultValue(type);
    }

    private void InitializePublicMembers(object instance, ParameterInfo[] constructorParameters)
    {
        var type = instance.GetType();
        var parameterNames = new HashSet<string>(
            constructorParameters.Select(p => p.Name)!,
            StringComparer.OrdinalIgnoreCase);

        foreach (var prop in type
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.CanWrite
                                 && p.SetMethod!.IsPublic
                                 && !parameterNames.Contains(p.Name)))
        {
            var generator = config?.GetGenerator(type, prop);
            object value;
            if (generator != null && generator.CanGenerate(prop.PropertyType))
                value = generator.Generate(prop.PropertyType, new(_random, this));
            else
                value = Create(prop.PropertyType);
            prop.SetValue(instance, value);
        }

        foreach (var field in
                 type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                     .Where(f => !f.IsInitOnly && !parameterNames.Contains(f.Name)))
        {
            var generator = config?.GetGenerator(type, field);
            object value;
            if (generator != null && generator.CanGenerate(field.FieldType))
                value = generator.Generate(field.FieldType, new(_random, this));
            else
                value = Create(field.FieldType);
            field.SetValue(instance, value);
        }
    }


    private object GenerateConstructorParameter(Type t, ParameterInfo constructorparameter)
    {
        
        var memberByName = FindMemberForParameter(t, constructorparameter);
        if (memberByName != null)
        {
            var generator = config?.GetGenerator(t, memberByName);
            if (generator != null && generator.CanGenerate(constructorparameter.ParameterType))
                return generator.Generate(constructorparameter.ParameterType, new(_random, this));
        }
        
        
        var membersByType = t.GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m is PropertyInfo or FieldInfo)
            .Where(m => GetMemberType(m) == constructorparameter.ParameterType);

        foreach (var member in membersByType)
        {
            var generator = config?.GetGenerator(t, member);
            if (generator != null && generator.CanGenerate(constructorparameter.ParameterType))
                return generator.Generate(constructorparameter.ParameterType, new GeneratorContext(_random, this));
        }

        
        return Create(constructorparameter.ParameterType);

        static MemberInfo? FindMemberForParameter(Type t, ParameterInfo parameter)
        {
            var paramName = parameter.Name;
            var paramType = parameter.ParameterType;

            return t.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m is PropertyInfo or FieldInfo)
                .FirstOrDefault(m =>
                    string.Equals(m.Name, paramName, StringComparison.OrdinalIgnoreCase) &&
                    GetMemberType(m) == paramType);
        }

        static Type GetMemberType(MemberInfo member) =>
            member switch
            {
                PropertyInfo p => p.PropertyType,
                FieldInfo f => f.FieldType,
                _ => throw new InvalidOperationException()
            };
    }


    private static object GetDefaultValue(Type t)
    {
        return t.IsValueType ?
            // Для типов-значений вызов конструктора по умолчанию даст default(T).
            Activator.CreateInstance(t)! :
            // Для ссылочных типов значение по умолчанию всегда null.
            null!;
    }
}