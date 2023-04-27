using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Voiceroid2Talker;

public class Voiceroid2Talker : DivinationPlugin<Voiceroid2Talker, PluginConfig>,
    IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>
{
    public Voiceroid2Talker(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        Dalamud.ChatGui.ChatMessage += OnChatReceived;
    }

    public string MainCommandPrefix => "/v2t";
    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

    [Command("/voiceroid2", "<text...>")]
    [CommandHelp("与えられた <text...> を読み上げます。")]
    private void OnTalkCommand(CommandContext context)
    {
        Divination.Voiceroid2Proxy.TalkAsync(context["text"]!);
    }

    private void OnChatReceived(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        try
        {
            if (Config.EnableTtsFcChatOnInactive && type == XivChatType.FreeCompany)
            {
                TtsFcChat(sender, message);
            }
        }
        catch (Exception ex)
        {
            PluginLog.Error(ex, "Error occurred in OnChatReceived");
        }
    }

    private void TtsFcChat(SeString sender, SeString message)
    {
        if (Win32Api.IsGameClientActive())
        {
            return;
        }

        var senderText = string.Join("", sender.Payloads.Select(x =>
        {
            switch (x)
            {
                case ITextProvider textProvider:
                    return textProvider.Text;
                case IconPayload:
                    return " ";
                default:
                    return string.Empty;
            }
        }));

        Divination.Voiceroid2Proxy.TalkAsync($"{senderText}: {message.TextValue}");
    }

    protected override void ReleaseManaged()
    {
        Dalamud.ChatGui.ChatMessage -= OnChatReceived;
    }
}
