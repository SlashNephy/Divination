using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Logger;

namespace Dalamud.Divination.Common.Api.Version
{
    internal partial class VersionManager : IVersionManager, ICommandProvider
    {
        private readonly IChatClient chatClient;
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug("VersionManager");

        public IGitVersion PluginVersion { get; }
        public IGitVersion LibraryVersion { get; }

        public VersionManager(IGitVersion pluginVersion, IGitVersion libraryVersion, IChatClient chatClient)
        {
            PluginVersion = pluginVersion;
            LibraryVersion = libraryVersion;
            this.chatClient = chatClient;
        }

        public void Dispose()
        {
            logger.Dispose();
        }
    }
}
