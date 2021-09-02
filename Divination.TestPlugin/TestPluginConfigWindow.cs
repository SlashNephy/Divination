using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.TestPlugin
{
    public sealed class TestPluginConfigWindow : ConfigWindow<TestConfig>
    {
        public override void Draw()
        {
            if (ImGui.Begin($"{TestPlugin.Instance.Name} Configuration", ref IsOpen, ImGuiWindowFlags.HorizontalScrollbar))
            {
                ImGui.Text("This is ConfigWindow.");

                ImGui.End();
            }
        }
    }
}
