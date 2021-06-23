using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration> : IConfigManager<TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
    {
        private readonly DalamudPluginInterface @interface;
        private readonly IChatClient chatClient;

        private TConfiguration? config;
        private readonly object configLock = new();
        public TConfiguration Config => config ?? throw new InvalidOperationException("Config はまだ初期化されていません。");

        public ConfigManager(DalamudPluginInterface @interface, IChatClient chatClient)
        {
            this.@interface = @interface;
            this.chatClient = chatClient;
        }

        public void LoadConfig()
        {
            lock (configLock)
            {
                config = @interface.GetPluginConfig() as TConfiguration ?? new TConfiguration();
            }
        }

        private static IEnumerable<FieldInfo> EnumerateConfigFields(bool includeUpdateIgnore = false)
        {
            var result = typeof(TConfiguration)
                .GetFields()
                .Where(x => x.FieldType == typeof(float) || x.FieldType == typeof(bool) || x.FieldType == typeof(int) || x.FieldType == typeof(string));

            return !includeUpdateIgnore ? result : result.Where(x => x.GetCustomAttribute<UpdateProhibitedAttribute>() == null);
        }

        public bool TryUpdate(string key, string? value)
        {
            var fields = EnumerateConfigFields(true);
            var updater = new FieldUpdater(Config, chatClient);

            return updater.TryUpdate(key, value, fields);
        }

        public void SaveConfig()
        {
            lock (configLock)
            {
                @interface.SavePluginConfig(config);
            }
        }

        public void Dispose()
        {
            SaveConfig();
        }
    }
}
