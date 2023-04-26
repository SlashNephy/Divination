using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;

namespace Dalamud.Divination.Common.Api.Version;

internal partial class VersionManager
{
    public class Commands : ICommandProvider
    {
        private readonly IChatClient chatClient;
        private readonly IVersionManager versionManager;

        public Commands(IVersionManager versionManager, IChatClient chatClient)
        {
            this.versionManager = versionManager;
            this.chatClient = chatClient;
        }

        [Command("version")]
        [CommandHelp("{Name} のバージョンを表示します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnVersionCommand()
        {
            chatClient.Print(versionManager.Plugin.InformationalVersion);
        }

        [Command("version", "common")]
        [CommandHelp("{Name} が使用している Dalamud.Divination.Common のバージョンを表示します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnVersionCommonCommand()
        {
            chatClient.Print(versionManager.Divination.InformationalVersion);
        }
    }
}
