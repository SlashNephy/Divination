using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Logger;

namespace Dalamud.Divination.Common.Api.Version
{
    internal partial class VersionManager
    {
        public class Commands : ICommandProvider
        {
            private readonly IVersionManager versionManager;
            private readonly IChatClient chatClient;

            public Commands(IVersionManager versionManager, IChatClient chatClient)
            {
                this.versionManager = versionManager;
                this.chatClient = chatClient;
            }

            [Command("Version", Help = "プラグインのバージョンを表示します。")]
            private void OnVersionCommand()
            {
                chatClient.Print(versionManager.PluginVersion.InformationalVersion);

                using var logger = DivinationLogger.Debug("VersionManager");
                logger.Debug("{Version}", versionManager.PluginVersion.ToString());
            }

            [Command("Version Library", Help = "プラグインが使用している Divination.Common のバージョンを表示します。")]
            private void OnVersionLibraryCommand()
            {
                chatClient.Print(versionManager.LibraryVersion.InformationalVersion);

                using var logger = DivinationLogger.Debug("VersionManager");
                logger.Debug("{Version}", versionManager.LibraryVersion.ToString());
            }
        }
    }
}
