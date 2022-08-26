using System.IO;
using System.Reflection;
using Dalamud.Configuration;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Config.Migration
{
    public static class ConfigMigrationTool
    {
        public static bool Merge<T>(string filename, T config) where T : class, IPluginConfiguration
        {
            var previousConfigPath =
                Path.Combine(DivinationEnvironment.XivLauncherDirectory, "pluginConfigs", filename);
            if (!File.Exists(previousConfigPath))
            {
                return false;
            }

            var previousConfig = DeserializeJsonFile<T>(previousConfigPath);
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var previousValue = field.GetValue(previousConfig);
                field.SetValue(config, previousValue);
            }

            return true;
        }

        private static T DeserializeJsonFile<T>(string path)
        {
            using var fs = File.OpenRead(path);
            using var sr = new StreamReader(fs);
            using var reader = new JsonTextReader(sr);

            var serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader)!;
        }
    }
}
