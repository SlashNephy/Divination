﻿using System.Text;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using Dalamud.Plugin;
using Divination.Debugger.Window;

namespace Divination.Debugger;

public partial class Debugger : DivinationPlugin<Debugger, PluginConfig>,
    IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>
{
    private readonly NetworkListener listener = new();

    public Debugger(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        Divination.ConfigWindow!.IsDrawing = Config.OpenAtStart;

        Dalamud.ChatGui.ChatMessage += OnChatMessage;
        Divination.Network.AddHandler(listener);
    }

    private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        if (Config.EnableVerboseChatLog)
        {
            var text = new StringBuilder();
            text.AppendLine($"[{type}, {isHandled}] {sender.TextValue} ({senderId}): {message.TextValue}");

            foreach (var payload in sender.Payloads)
            {
                text.AppendLine($"  {payload}");
            }

            foreach (var payload in message.Payloads)
            {
                text.AppendLine($"    {payload}");
            }

            DalamudLog.Log.Verbose("{Chat}", text.ToString());
        }
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        Dalamud.ChatGui.ChatMessage -= OnChatMessage;
        Divination.Network.RemoveHandler(listener);
        listener.Dispose();
    }

    public string MainCommandPrefix => "/debug";
    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();
}
