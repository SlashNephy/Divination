using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Logging;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration> : IConfigManager<TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
    {
        private readonly IChatClient chatClient;
        private readonly Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient;
        private readonly string pluginName;
        private readonly PluginConfigurations dalamud = new(DivinationEnvironment.XivLauncherPluginConfigDirectory);

        public TConfiguration Config { get; }

        public ConfigManager(IChatClient chatClient, Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient, string pluginName)
        {
            this.chatClient = chatClient;
            this.voiceroid2ProxyClient = voiceroid2ProxyClient;
            this.pluginName = pluginName;

            Config = dalamud.LoadForType<TConfiguration>(pluginName);
            PluginLog.Verbose("Config loaded: {Config}", JsonConvert.SerializeObject(Config));
        }

        private static IEnumerable<FieldInfo> EnumerateConfigFields(bool includeUpdateIgnore = false)
        {
            var result = typeof(TConfiguration)
                .GetFields()
                .Where(x => x.FieldType == typeof(float) || x.FieldType == typeof(bool) || x.FieldType == typeof(int) || x.FieldType == typeof(string));

            return !includeUpdateIgnore ? result : result.Where(x => x.GetCustomAttribute<UpdateProhibitedAttribute>() == null);
        }

        public bool TryUpdate(string key, string? value, bool useTts)
        {
            var updater = new FieldUpdater(Config, chatClient, voiceroid2ProxyClient.Invoke(), useTts);

            var fields = EnumerateConfigFields(true);
            return updater.TryUpdate(key, value, fields);
        }

        public void Save()
        {
            dalamud.Save(Config, pluginName);
            PluginLog.Verbose("Config saved: {Config}", JsonConvert.SerializeObject(Config));
        }

        public void Dispose()
        {
            Save();
        }
    }
}
