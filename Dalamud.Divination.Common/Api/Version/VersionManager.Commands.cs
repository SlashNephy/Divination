using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;

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

            [Command("version")]
            [CommandHelp("プラグインのバージョンを表示します。")]
            private void OnVersionCommand()
            {
                chatClient.Print(versionManager.Plugin.InformationalVersion);
            }

            [Command("version", "common")]
            [CommandHelp("プラグインが使用している Divination.Common のバージョンを表示します。")]
            private void OnVersionCommonCommand()
            {
                chatClient.Print(versionManager.Divination.InformationalVersion);
            }
        }
    }
}
