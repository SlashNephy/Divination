using System;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Divination.Common.Api.Input;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Divination.Common.Api.XivApi;

namespace Dalamud.Divination.Common.Api;

public interface IDivinationApi<TConfiguration, out TDefinition> : IDisposable where TConfiguration : class, IPluginConfiguration, new()
    where TDefinition : DefinitionContainer
{
    public IChatClient Chat { get; }

    public ICommandProcessor? Command { get; }

    public ITextureManager Texture { get; }

    public IVersionManager Version { get; }

    public IVoiceroid2ProxyClient Voiceroid2Proxy { get; }

    public IXivApiClient XivApi { get; }

    public IKeyStrokeManager KeyStroke { get; }

    public IConfigManager<TConfiguration> Config { get; }

    public ConfigWindow<TConfiguration>? ConfigWindow { get; }

    public IDefinitionManager<TDefinition>? Definition { get; }
}
