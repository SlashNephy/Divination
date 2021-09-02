using System;
using System.Collections.Generic;

namespace Dalamud.Divination.Common.Api
{
    internal static class ServiceContainer
    {
        private static readonly Dictionary<Type, object?> Services = new();

        public static T GetOrPut<T>(Func<T> initializer) where T : class
        {
            if (Services.TryGetValue(typeof(T), out var service))
            {
                return service as T ?? throw new InvalidCastException("Cast failed.");
            }

            var newService = initializer();
            Services[typeof(T)] = newService;
            return newService;
        }

        public static T? GetOrPutOptional<T>(Func<T?> initializer) where T : class?
        {
            if (Services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }

            var newService = initializer();
            Services[typeof(T)] = newService;
            return newService;
        }
    }
}
