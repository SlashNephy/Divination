using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Config;

internal partial class ConfigManager<TConfiguration> : IConfigManager<TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
{
    private readonly IChatClient chatClient;
    private readonly IDivinationPluginApi<TConfiguration, DefinitionContainer> pluginApi;
    private readonly Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient;

    public ConfigManager(IDivinationPluginApi<TConfiguration, DefinitionContainer> pluginApi,
        IDalamudPluginInterface pluginInterface,
        IChatClient chatClient,
        Func<IVoiceroid2ProxyClient> voiceroid2ProxyClient)
    {
        this.pluginApi = pluginApi;
        Interface = pluginInterface;
        this.chatClient = chatClient;
        this.voiceroid2ProxyClient = voiceroid2ProxyClient;
    }

    // ベースクラス初期化後 → プラグインクラス初期化なので、ロード直後は Config 利用不可
    public TConfiguration Config
    {
        get
        {
            if (pluginApi.Config == default)
            {
                throw new AggregateException("Config has not initialized yet.");
            }

            return pluginApi.Config;
        }
    }

    public IDalamudPluginInterface Interface { get; }

    public bool TryUpdate(string key, string? value, bool useTts)
    {
        var updater = new FieldUpdater(Config, chatClient, voiceroid2ProxyClient.Invoke(), useTts);

        var fields = EnumerateConfigFields(true);
        return updater.TryUpdate(key, value, fields);
    }

    private static IEnumerable<FieldInfo> EnumerateConfigFields(bool includeUpdateIgnore = false)
    {
        var result = typeof(TConfiguration).GetFields()
            .Where(x => x.FieldType == typeof(float) || x.FieldType == typeof(bool) || x.FieldType == typeof(int) || x.FieldType == typeof(string));

        return !includeUpdateIgnore ? result : result.Where(x => x.GetCustomAttribute<UpdateProhibitedAttribute>() == null);
    }
}
