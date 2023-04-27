using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Logging;
using ImGuiNET;

namespace Divination.ChatFilter;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{ChatFilterPlugin.Instance.Name} Config", ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text("This is config window.");

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;

                ChatFilterPlugin.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                PluginLog.Information("Config saved");
            }

            ImGui.End();
        }
    }
}