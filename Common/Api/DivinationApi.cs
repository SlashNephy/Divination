using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Divination.Common.Api.Input;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Divination.Common.Api.XivApi;
using Dalamud.Divination.Common.Boilerplate.Features;

namespace Dalamud.Divination.Common.Api;

internal sealed class DivinationApi<TConfiguration, TDefinition> : IDivinationApi<TConfiguration, TDefinition>
    where TConfiguration : class, IPluginConfiguration, new()
    where TDefinition : DefinitionContainer, new()
{
    public DivinationApi(IDalamudApi api, Assembly assembly, IDivinationPluginApi<TConfiguration, TDefinition> plugin)
    {
        Dalamud = api;
        ServiceContainer.Put(Dalamud);
        Assembly = assembly;
        Plugin = plugin;

        Command?.RegisterCommandsByAttribute(new VersionManager.Commands(Version, Chat));
        if (Definition != null)
        {
            Command?.RegisterCommandsByAttribute(new DefinitionManager<TDefinition>.Commands(Definition));
        }

        Command?.RegisterCommandsByAttribute(new ConfigManager<TConfiguration>.Commands(Config, Command, Chat));
        Command?.RegisterCommandsByAttribute(ConfigWindow);
    }

    private IDalamudApi Dalamud { get; }
    private Assembly Assembly { get; }
    private IDivinationPluginApi<TConfiguration, TDefinition> Plugin { get; }

    public IChatClient Chat => ServiceContainer.GetOrPut(() => new ChatClient(Plugin.Name, Dalamud.ChatGui, Dalamud.ClientState));

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public ICommandProcessor? Command => ServiceContainer.GetOrPutOptional(() =>
    {
        var processor = Plugin switch
        {
            ICommandSupport commandSupport => new CommandProcessor(Plugin.Name,
                commandSupport.MainCommandPrefix,
                Dalamud.ChatGui,
                Chat,
                Dalamud.CommandManager),
            ICommandProvider => new CommandProcessor(Plugin.Name, null, Dalamud.ChatGui, Chat, Dalamud.CommandManager),
            _ => null,
        };

        if (processor == null)
        {
            return null;
        }

        processor.RegisterCommandsByAttribute(new DirectoryCommands());
        processor.RegisterCommandsByAttribute((ICommandProvider)Plugin);
        return processor;
    });

    public IConfigManager<TConfiguration> Config => ServiceContainer.GetOrPut(() =>
        new ConfigManager<TConfiguration>(Plugin, Dalamud.PluginInterface, Chat, () => Voiceroid2Proxy));
    public ConfigWindow<TConfiguration>? ConfigWindow => ServiceContainer.GetOrPutOptional(() =>
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (Plugin is IConfigWindowSupport<TConfiguration> configWindowSupport)
        {
            var window = configWindowSupport.CreateConfigWindow();
            window.ConfigManager = Config;
            window.UiBuilder = Dalamud.PluginInterface.UiBuilder;
            window.EnableHook();

            return window;
        }

        return null;
    });

    public IDefinitionManager<TDefinition>? Definition => ServiceContainer.GetOrPutOptional(() =>
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (Plugin is IDefinitionSupport definitionSupport)
        {
            return new DefinitionManager<TDefinition>(definitionSupport.DefinitionUrl, Chat, () => Voiceroid2Proxy);
        }

        return null;
    });

    public ITextureManager Texture => ServiceContainer.GetOrPut(() => new TextureManager(Dalamud.TextureProvider, Dalamud.PluginInterface.UiBuilder));
    public IVersionManager Version =>
        ServiceContainer.GetOrPut(() => new VersionManager(new GitVersion(Assembly), new GitVersion(Assembly.GetExecutingAssembly())));
    public IVoiceroid2ProxyClient Voiceroid2Proxy => ServiceContainer.GetOrPut(() => new Voiceroid2ProxyClient());
    public IXivApiClient XivApi => ServiceContainer.GetOrPut(() => new XivApiClient());
    public IKeyStrokeManager KeyStroke => ServiceContainer.GetOrPut(() => new KeyStrokeManager());

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServiceContainer.DestroyAll();
        }
    }

    ~DivinationApi()
    {
        Dispose(false);
    }

    #endregion
}
