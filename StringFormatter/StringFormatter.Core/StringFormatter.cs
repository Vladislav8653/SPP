using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StringFormatter.Core;

public class StringFormatter : IStringFormatter
{
    public static StringFormatter Shared { get; } = new();
    private static readonly ConcurrentDictionary<string, Func<object, object?>> PropertyCache = new();

    public string Format(string template, object target)
    {
        if (template is null) throw new ArgumentNullException(nameof(template));
        if (target is null) throw new ArgumentNullException(nameof(target));

        var type = target.GetType();
        var result = new StringBuilder();
        int pos = 0;

        while (pos < template.Length)
        {
            int open = template.IndexOf('{', pos);
            if (open < 0)
            {
                result.Append(template, pos, template.Length - pos);
                break;
            }
            
            if (open + 1 < template.Length && template[open + 1] == '{')
            {
                result.Append(template, pos, open - pos + 1);
                pos = open + 2;
                continue;
            }

            result.Append(template, pos, open - pos);
            int close = template.IndexOf('}', open + 1);
            if (close < 0)
                throw new FormatException("Unbalanced curly braces in format string.");
            
            if (close + 1 < template.Length && template[close + 1] == '}')
            {
                result.Append(template, open, close - open + 2);
                pos = close + 2;
                continue;
            }

            string propertyPath = template.Substring(open + 1, close - open - 1);
            result.Append(Format_EvaluateProperty(target, type, propertyPath));
            pos = close + 1;
        }

        return result.ToString();
    }

    private static string Format_EvaluateProperty(object target, Type type, string propertyPath)
    {
        string cacheKey = $"{type.FullName}:{propertyPath}";
        var accessor = PropertyCache.GetOrAdd(cacheKey, _ => Format_BuildAccessor(type, propertyPath));
        var value = accessor(target);
        return value?.ToString() ?? string.Empty;
    }

    private static Func<object, object?> Format_BuildAccessor(Type type, string propertyPath)
    {
        string[] segments = propertyPath.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
        ParameterExpression param = Expression.Parameter(typeof(object), "obj");
        Expression body = Expression.Convert(param, type);
        Type currentType = type;

        foreach (var segment in segments)
        {
            if (int.TryParse(segment, out int index))
            {
                if (currentType.IsArray)
                {
                    body = Expression.ArrayIndex(body, Expression.Constant(index));
                    currentType = currentType.GetElementType()!;
                }
                else if (typeof(System.Collections.IList).IsAssignableFrom(currentType))
                {
                    var indexer = currentType.GetProperty("Item");
                    body = Expression.Property(body, indexer!, Expression.Constant(index));
                    currentType = indexer!.PropertyType;
                }
                else
                {
                    throw new FormatException($"Indexing not supported for type {currentType.Name}.");
                }
            }
            else
            {
                var prop = currentType.GetProperty(segment, BindingFlags.Instance | BindingFlags.Public)
                           ?? throw new FormatException($"Property '{segment}' not found in type '{currentType.Name}'.");
                body = Expression.Property(body, prop);
                currentType = prop.PropertyType;
            }
        }
        Expression converted = Expression.Convert(body, typeof(object));
        var lambda = Expression.Lambda<Func<object, object?>>(converted, param);
        return lambda.Compile();
    }
}
