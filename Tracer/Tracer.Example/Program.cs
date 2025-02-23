using Core;
using Tracer.Serialization;

namespace Example;
class Program
{
    private static void Main()
    {
        var tracer = new Core.Tracer();
        var tasks = new List<Task>();
        for (var i = 0; i < 10; i++)
        {
            var task = Task.Run(() =>
            {
                Foo foo = new Foo(tracer);
                foo.MyMethod();
            });
            tasks.Add(task);
        }
        Task.WhenAll(tasks).Wait();
        var result = tracer.GetTraceResult();
        var serializers = PluginLoader.LoadPlugins();
        foreach (var serializer in serializers)
        {
            var fileName = Path.Combine("Tracers", $"tracer.{serializer.Format}");
            using var fileStream = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(result, fileStream);
        }
    }
}

public class Foo
{
    private Bar _bar;
    private ITracer _tracer;

    internal Foo(ITracer tracer)
    {
        _tracer = tracer;
        _bar = new Bar(_tracer);
    }
    
    public void MyMethod()
    {
        _tracer.StartTrace();
        Console.WriteLine("Outer method start");
         Thread.Sleep(100);
        _bar.InnerMethod(200);
        _tracer.StopTrace();
        Console.WriteLine("Outer method finish");
    }
}

public class Bar
{
    private readonly ITracer _tracer;

    public Bar(ITracer tracer)
    {
        _tracer = tracer;
    }
    
    public void InnerMethod(int time)
    {
        _tracer.StartTrace();
        Console.WriteLine("Inner method start");
        Thread.Sleep(time);
        _tracer.StopTrace();
        Console.WriteLine("Inner method finish");
    } 
}
