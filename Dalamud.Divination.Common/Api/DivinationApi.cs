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
            // ReSharper disable once SuspiciousTypeConversion.Global
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
            // ReSharper disable once SuspiciousTypeConversion.Global
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
            // ReSharper disable once SuspiciousTypeConversion.Global
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
