using System;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Divination.Common.Api.Input;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Divination.Common.Api.XivApi;
using Dalamud.Divination.Common.Boilerplate.Features;

namespace Dalamud.Divination.Common.Api
{
    internal sealed class DivinationApi<TConfiguration, TDefinition> : IDivinationApi<TConfiguration, TDefinition>, IDisposable
        where TConfiguration : class, IPluginConfiguration, new()
        where TDefinition : DefinitionContainer, new()
    {
        private IDalamudApi Dalamud { get; }
        private Assembly Assembly { get; }
        private IDivinationPluginApi<TConfiguration, TDefinition> Plugin { get; }

        public IChatClient Chat => ServiceContainer.GetOrPut(() => new ChatClient(Plugin.Name, Dalamud.ChatGui));

        public ICommandProcessor? Command => ServiceContainer.GetOrPutOptional(() =>
        {
            if (Plugin is ICommandSupport commandSupport)
            {
                var processor = new CommandProcessor(Plugin.Name, commandSupport.MainCommandPrefix, Dalamud.ChatGui, Chat);
                processor.RegisterCommandsByAttribute(new DirectoryCommands());
                processor.RegisterCommandsByAttribute(commandSupport);
                return processor;
            }

            return null;
        });

        public IConfigManager<TConfiguration> Config => ServiceContainer.GetOrPut(() =>
        {
            var manager = new ConfigManager<TConfiguration>(Dalamud.PluginInterface, Chat);
            Command?.RegisterCommandsByAttribute(new ConfigManager<TConfiguration>.Commands(manager, Command, Chat));
            return manager;
        });

        public ConfigWindow<TConfiguration>? ConfigWindow => ServiceContainer.GetOrPutOptional(() =>
        {
            if (Plugin is IConfigWindowSupport<TConfiguration> configWindowSupport)
            {
                var window = configWindowSupport.CreateConfigWindow();
                window.ConfigManager = Config;
                Command?.RegisterCommandsByAttribute(window);
                Dalamud.PluginInterface.UiBuilder.Draw += window.OnDraw;

                return window;
            }

            return null;
        });

        public IDefinitionManager<TDefinition>? Definition => ServiceContainer.GetOrPutOptional(() =>
        {
            if (Plugin is IDefinitionSupport definitionSupport)
            {
                var manager = new DefinitionManager<TDefinition>(definitionSupport.DefinitionUrl, Chat);
                Command?.RegisterCommandsByAttribute(new DefinitionManager<TDefinition>.Commands(manager));

                return manager;
            }

            return null;
        });

        public IBugReporter Reporter => ServiceContainer.GetOrPut(() =>
        {
            var reporter = new BugReporter(Plugin.Name, Version, Chat);
            Command?.RegisterCommandsByAttribute(new BugReporter.Commands(reporter));
            return reporter;
        });

        public ITextureManager Texture => ServiceContainer.GetOrPut(() => new TextureManager(Dalamud.DataManager, Dalamud.PluginInterface.UiBuilder));
        public IVersionManager Version => ServiceContainer.GetOrPut(() =>
        {
            var manager = new VersionManager(
                new GitVersion(Assembly),
                new GitVersion(Assembly.GetExecutingAssembly()));
            Command?.RegisterCommandsByAttribute(new VersionManager.Commands(Version, Chat));
            return manager;
        });

        public IVoiceroid2ProxyClient Voiceroid2Proxy => ServiceContainer.GetOrPut(() => new Voiceroid2ProxyClient());
        public IXivApiClient XivApi => ServiceContainer.GetOrPut(() => new XivApiClient());
        public IKeyStrokeManager KeyStroke => ServiceContainer.GetOrPut(() => new KeyStrokeManager());
        public INetworkInterceptor Network => ServiceContainer.GetOrPut(() => new NetworkInterceptor(Dalamud.GameNetwork));

        public DivinationApi(IDalamudApi api, Assembly assembly, IDivinationPluginApi<TConfiguration, TDefinition> plugin)
        {
            Dalamud = api;
            Assembly = assembly;
            Plugin = plugin;

            // Chat = new ChatClient(plugin.Name, api.ChatGui);
            //
            // if (plugin is ICommandSupport commandSupport)
            // {
            //     Command = new CommandProcessor(plugin.Name, commandSupport.MainCommandPrefix, api.ChatGui, Chat);
            //     Command.RegisterCommandsByAttribute(new DirectoryCommands());
            //
            //     Command.RegisterCommandsByAttribute(commandSupport);
            // }
            //
            // Config = new ConfigManager<TConfiguration>(api.PluginInterface, Chat);
            // Command?.RegisterCommandsByAttribute(new ConfigManager<TConfiguration>.Commands(Config, Command, Chat));
            //
            // if (plugin is IConfigWindowSupport<TConfiguration> configWindowSupport)
            // {
            //     ConfigWindow = configWindowSupport.CreateConfigWindow();
            //     ConfigWindow.ConfigManager = Config;
            //     Command?.RegisterCommandsByAttribute(ConfigWindow);
            //
            //     api.PluginInterface.UiBuilder.Draw += ConfigWindow.OnDraw;
            // }
            //
            // if (plugin is IDefinitionSupport definitionSupport)
            // {
            //     Definition = new DefinitionManager<TDefinition>(definitionSupport.DefinitionUrl, Chat);
            //     Command?.RegisterCommandsByAttribute(new DefinitionManager<TDefinition>.Commands(Definition));
            // }
            //
            // Version = new VersionManager(
            //     new GitVersion(assembly),
            //     new GitVersion(Assembly.GetExecutingAssembly()));
            // Command?.RegisterCommandsByAttribute(new VersionManager.Commands(Version, Chat));
            //
            // Texture = new TextureManager(api.DataManager, api.PluginInterface.UiBuilder);
            //
            // Voiceroid2Proxy = new Voiceroid2ProxyClient();
            //
            // XivApi = ;
            //
            // KeyStroke = ;
            //
            // Network = ;
            //
            // Reporter = new BugReporter(plugin.Name, Version, Chat);
            // Command?.RegisterCommandsByAttribute(new BugReporter.Commands(Reporter));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Config.Dispose();

                if (ConfigWindow != null)
                {
                    Dalamud.PluginInterface.UiBuilder.Draw -= ConfigWindow.OnDraw;
                }

                Version.Dispose();
                Command?.Dispose();
                Texture.Dispose();
                Definition?.Dispose();
                Voiceroid2Proxy.Dispose();
                XivApi.Dispose();
                Reporter.Dispose();
                Chat.Dispose();
            }
        }

        ~DivinationApi()
        {
            Dispose(false);
        }

        #endregion
    }
}
