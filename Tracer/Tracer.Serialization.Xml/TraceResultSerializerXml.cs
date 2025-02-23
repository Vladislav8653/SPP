using System.Xml.Serialization;
using Abstractions;
using Core;

namespace Xml;

public class TraceResultSerializerXml : ITraceResultSerializer
{
    public string Format { get; } = "xml";
    public void Serialize(TraceResult traceResult, Stream to)
    {
        var serializer = new XmlSerializer(typeof(TraceResult));
        using (var writer = new StreamWriter(to))
        {
            serializer.Serialize(writer, traceResult);
        }
    }
}