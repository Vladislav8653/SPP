using System.Text.Json;
using Abstractions;
using Core;

namespace Json;

public class TraceResultSerializerJson : ITraceResultSerializer
{
    public string Format { get; } = "json";
    public void Serialize(TraceResult traceResult, Stream to)
    {
        using (var writer = new StreamWriter(to))
        {
            var json = JsonSerializer.Serialize(traceResult, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(json);
        }
    }
}