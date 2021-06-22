using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Api.Version
{
    internal partial class VersionManager
    {
        public object GetCommandInstance()
        {
            return new Commands(this);
        }

        private class Commands
        {
            private readonly VersionManager versionManager;

            public Commands(VersionManager versionManager)
            {
                this.versionManager = versionManager;
            }

            [Command("Version", Help = "プラグインのバージョンを表示します。")]
            private void OnVersionCommand()
            {
                versionManager.chatClient.Print(versionManager.PluginVersion.InformationalVersion);

                versionManager.logger.Debug("{Version}", versionManager.PluginVersion.ToString());
            }

            [Command("Version Library", Help = "プラグインが使用している Divination.Common のバージョンを表示します。")]
            private void OnVersionLibraryCommand()
            {
                versionManager.chatClient.Print(versionManager.LibraryVersion.InformationalVersion);

                versionManager.logger.Debug("{Version}", versionManager.LibraryVersion.ToString());
            }
        }
    }
}
