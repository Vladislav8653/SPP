using System.Reflection;
using Abstractions;

namespace Tracer.Serialization;

public static class PluginLoader
{
    public static IEnumerable<ITraceResultSerializer> LoadPlugins()
    {
        var plugins = new List<ITraceResultSerializer>();
        var pluginsDir = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        if (!Directory.Exists(pluginsDir))
        {
            Console.WriteLine("Plugins directory doesn't exist");
            return plugins;
        }
        var pluginFiles = Directory.GetFiles(pluginsDir, "*.dll");
        foreach (var pluginFile in pluginFiles)
        {
            var assembly = Assembly.LoadFrom(pluginFile);
            var types = assembly.GetTypes()
                .Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is ITraceResultSerializer plugin)
                {
                    plugins.Add(plugin);
                }
            }
        }
        return plugins;
    }
}