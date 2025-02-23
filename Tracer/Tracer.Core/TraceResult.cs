using System.Collections.Immutable;

namespace Core;

public class TraceResult
{
    public required ImmutableList<ThreadInfo> Threads { get; init; }
}