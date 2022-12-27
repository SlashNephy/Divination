using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin(Localization.ConfigWindowTitle.Format(AetheryteLinkInChatPlugin.Instance.Name), ref IsOpen, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize))
        {
            ImGui.Checkbox(Localization.AllowTeleportQueueing.ToString(), ref Config.AllowTeleportQueueing);
            if (Config.AllowTeleportQueueing)
            {
                ImGui.SliderInt(Localization.QueueTeleportDelay.ToString(), ref Config.QueuedTeleportDelay, 250, 5000);
            }

            ImGui.Indent();
            ImGui.Text(Localization.QueuedTeleportDescription.ToString());
            ImGui.Unindent();

            ImGui.Spacing();

            if (ImGui.Button(Localization.SaveConfigButton.ToString()))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }
}
