using Abstractions;
using Core;
using YamlDotNet.Serialization;

namespace Yaml;

public class TraceResultSerializerYaml : ITraceResultSerializer
{
    public string Format { get; } = "yaml";
    public void Serialize(TraceResult traceResult, Stream to)
    {
        var serializer = new Serializer();
        using (var writer = new StreamWriter(to))
        {
            serializer.Serialize(writer, traceResult);
        }
    }
}