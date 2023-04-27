using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.PerfectComplex;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin(PerfectComplex.Instance.Name, ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
        {
            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }
}
