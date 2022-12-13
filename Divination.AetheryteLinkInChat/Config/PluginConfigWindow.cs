using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{AetheryteLinkInChatPlugin.Instance.Name} Config", ref IsOpen,
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize))
        {
            ImGui.Checkbox("Print Aetheryte Name to Chat", ref Config.PrintAetheryteName);

            ImGui.Checkbox("Allow Teleport Queueing", ref Config.AllowTeleportQueueing);
            if (Config.AllowTeleportQueueing)
            {
                ImGui.SliderInt("Queued Teleport Delay (ms)", ref Config.QueuedTeleportDelay, 500, 5000);
            }

            ImGui.Indent();
            ImGui.Text("It can queue when teleport is unavailable, e.g. in combat.");
            ImGui.Text("Then your teleport will be executed after the Delay.");
            ImGui.Unindent();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }
}
