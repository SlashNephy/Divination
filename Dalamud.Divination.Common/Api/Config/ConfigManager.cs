using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Logging;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration> : IConfigManager<TConfiguration>
        where TConfiguration : class, IPluginConfiguration, new()
    {
        private readonly IChatClient chatClient;
        private readonly DalamudPluginInterface pluginInterface;
        private readonly Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient;

        public ConfigManager(
            DalamudPluginInterface pluginInterface,
            IChatClient chatClient,
            Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient)
        {
            this.pluginInterface = pluginInterface;
            this.chatClient = chatClient;
            this.voiceroid2ProxyClient = voiceroid2ProxyClient;

            Config = pluginInterface.GetPluginConfig() as TConfiguration ?? new TConfiguration();
            PluginLog.Verbose("Config loaded: {Config}", JsonConvert.SerializeObject(Config));
        }

        public TConfiguration Config { get; }

        public bool TryUpdate(string key, string? value, bool useTts)
        {
            var updater = new FieldUpdater(Config, chatClient, voiceroid2ProxyClient.Invoke(), useTts);

            var fields = EnumerateConfigFields(true);
            return updater.TryUpdate(key, value, fields);
        }

        public void Save()
        {
            pluginInterface.SavePluginConfig(Config);
            PluginLog.Verbose("Config saved: {Config}", JsonConvert.SerializeObject(Config));
        }

        public void Dispose()
        {
            // Save();
        }

        private static IEnumerable<FieldInfo> EnumerateConfigFields(bool includeUpdateIgnore = false)
        {
            var result = typeof(TConfiguration).GetFields()
                .Where(x => x.FieldType == typeof(float) || x.FieldType == typeof(bool) || x.FieldType == typeof(int) ||
                            x.FieldType == typeof(string));

            return !includeUpdateIgnore
                ? result
                : result.Where(x => x.GetCustomAttribute<UpdateProhibitedAttribute>() == null);
        }
    }
}
