using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Logging;

namespace Dalamud.Divination.Common.Api;

internal static class ServiceContainer
{
    private static readonly List<(Type type, object? instance)> Services = new();

    public static T? Get<T>() where T : class
    {
        lock (Services)
        {
            var entry = Services.FirstOrDefault(x => x.type == typeof(T));
            if (entry == default)
            {
                return default;
            }

            return entry.instance as T;
        }
    }

    public static void Put<T>(T instance) where T : class
    {
        lock (Services)
        {
            Services.Add((typeof(T), instance));
        }
    }

    public static T GetOrPut<T>(Func<T> initializer) where T : class
    {
        return GetOrPutOptional(initializer) ?? throw new InvalidCastException("Cast failed.");
    }

    public static T? GetOrPutOptional<T>(Func<T?> initializer) where T : class?
    {
        lock (Services)
        {
            var entry = Services.FirstOrDefault(x => x.type == typeof(T));
            // PluginLog.Verbose($"Get<{typeof(T)}>: {entry.instance?.GetType()}");

            if (entry == default)
            {
                entry = (typeof(T), initializer());
                Services.Add(entry);
                PluginLog.Verbose($"Put<{entry.type}>: {entry.instance?.GetType()}");
            }

            return entry.instance as T;
        }
    }

    public static bool Contains<T>() where T : class
    {
        lock (Services)
        {
            return Services.Any(x => x.type == typeof(T));
        }
    }

    public static void DestroyAll()
    {
        // 追加された順の逆順で破棄する
        lock (Services)
        {
            foreach (var (type, instance) in Services)
            {
                if (instance is IDisposable disposable)
                {
                    disposable.Dispose();
                    PluginLog.Verbose("Dispose: {Disposable}", type.FullName!);
                }
            }

            Services.Clear();
            PluginLog.Verbose("ServiceContainer is clear");
        }
    }
}

