using System.Text;

namespace Core;

public class ThreadInfo()
{
    public required int ThreadId { get; init; } 
    public long Time { get; set; }
    public List<MethodInfo> Methods { get; set; } = [];

    public override bool Equals(object? obj)
    {
        if (obj is ThreadInfo other)
        {
            return ThreadId == other.ThreadId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return ThreadId.GetHashCode();
    }
    
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append($"Thread {ThreadId}, Time: {Time}");
        foreach (var method in Methods)
        {
            builder.Append($"\n{method}");
        }

        return builder.ToString();
    }
}