using System;
using System.Collections.Generic;
using Dalamud.Logging;

namespace Dalamud.Divination.Common.Api
{
    internal static class ServiceContainer
    {
        private static readonly Dictionary<Type, object?> Services = new();

        public static T GetOrPut<T>(Func<T> initializer) where T : class
        {
            lock (Services)
            {
                if (Services.TryGetValue(typeof(T), out var service))
                {
                    PluginLog.Debug($"Get<{typeof(T)}>: {service?.GetType()}");
                    return service as T ?? throw new InvalidCastException("Cast failed.");
                }

                var newService = initializer();
                Services[typeof(T)] = newService;
                PluginLog.Debug($"Put<{typeof(T)}>: {newService.GetType()}");
                return newService;
            }
        }

        public static T? GetOrPutOptional<T>(Func<T?> initializer) where T : class?
        {
            lock (Services)
            {
                if (Services.TryGetValue(typeof(T), out var service))
                {
                    PluginLog.Debug($"Get<{typeof(T)}>: {service?.GetType()}");
                    return service as T;
                }

                var newService = initializer();
                Services[typeof(T)] = newService;
                PluginLog.Debug($"Put<{typeof(T)}>: {newService?.GetType()}");
                return newService;
            }
        }
    }
}
