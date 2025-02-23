using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Core;

public class Tracer : ITracer
{
    private readonly ConcurrentDictionary<int, ThreadInfo> _threads;
    private readonly ThreadLocal<Stack<MethodInfo>> _methods;
    private readonly object _lock = new object();


    public Tracer()
    {
        _methods = new ThreadLocal<Stack<MethodInfo>>(() => new Stack<MethodInfo>());
        _threads = new ConcurrentDictionary<int, ThreadInfo>();
    }

    public void StartTrace()
    {
        if (_methods.Value is null)
            return;
        var stackTrace = new StackTrace();
        var methodName = stackTrace.GetFrame(1)?.GetMethod()?.Name;
        if (methodName == null) throw new NullReferenceException("Method name was null");
        var className = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType?.Name;
        if (className == null) throw new NullReferenceException("Class name was null");
        var methodInfo = new MethodInfo(new Stopwatch())
        {
            ClassName = className,
            MethodName = methodName
        };
        methodInfo.StartTimer();
        _methods.Value.Push(methodInfo);
    }

    public void StopTrace()
    {
        if (_methods.Value is null)
            return;
        MethodInfo methodInfo;
        if (_methods.Value.Count == 0) return;
        methodInfo = _methods.Value.Pop();
        methodInfo.StopTimer();
        var currentThreadId = Environment.CurrentManagedThreadId;
        var currentThread = _threads.GetOrAdd(currentThreadId, id => new ThreadInfo { ThreadId = id });
        lock (_lock)
        {
            currentThread.Time += methodInfo.Time;
            if (_methods.Value.Count > 0)
            {
                var parentMethodInfo = _methods.Value.Peek();
                parentMethodInfo.Methods.Add(methodInfo);
            }
            else
            {
                currentThread.Methods.Add(methodInfo);
            }
        }
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult { Threads = _threads.Values.ToImmutableList() };
    }
}