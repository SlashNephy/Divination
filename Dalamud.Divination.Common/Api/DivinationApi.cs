using System;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Divination.Common.Boilerplate;

namespace Dalamud.Divination.Common.Api
{
    public sealed class DivinationApi<TConfiguration> : IDivinationApi, IDivinationApi<TConfiguration>, IDisposable
        where TConfiguration : class, IPluginConfiguration, new()
    {
        public IVersionManager VersionManager { get; }
        public IChatClient ChatClient { get; }
        public ICommandProcessor? CommandProcessor { get; }
        public IConfigManager<TConfiguration> ConfigManager { get; }
        public IBugReporter BugReporter { get; }

        public DivinationApi(IDalamudApi api, Assembly assembly, IDivinationPluginApi<TConfiguration> plugin)
        {
            ChatClient = new ChatClient(plugin.Name, api.ChatGui);

            if (plugin is ICommandSupport support)
            {
                CommandProcessor = new CommandProcessor(plugin.Name, support.CommandPrefix, api.ChatGui, ChatClient);
                CommandProcessor.RegisterCommandsByAttribute(new DirectoryCommands());

                CommandProcessor.RegisterCommandsByAttribute(support);
            }

            VersionManager = new VersionManager(
                new GitVersion(assembly),
                new GitVersion(Assembly.GetExecutingAssembly()));
            CommandProcessor?.RegisterCommandsByAttribute(new VersionManager.Commands(VersionManager, ChatClient));

            ConfigManager = new ConfigManager<TConfiguration>(api.PluginInterface, ChatClient);
            CommandProcessor?.RegisterCommandsByAttribute(new ConfigManager<TConfiguration>.Commands(ConfigManager, CommandProcessor, ChatClient));

            BugReporter = new BugReporter(plugin.Name, VersionManager, ChatClient);
            CommandProcessor?.RegisterCommandsByAttribute(new BugReporter.Commands(BugReporter));
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
                ConfigManager.Dispose();
                VersionManager.Dispose();
                CommandProcessor?.Dispose();
                BugReporter.Dispose();
                ChatClient.Dispose();
            }
        }

        ~DivinationApi()
        {
            Dispose(false);
        }

        #endregion
    }
}
