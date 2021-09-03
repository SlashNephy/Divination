using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.AetheryteLinkInChat
{
    public class PluginConfigWindow : ConfigWindow<PluginConfig>
    {
        public override void Draw()
        {
            if (ImGui.Begin($"{AetheryteLinkInChatPlugin.Instance.Name} 設定", ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGuiEx.CheckboxConfig("チャットの座標に最寄りのエーテライトを表示する", ref Config.Enabled,
                    "クリックすると該当のエーテライトにテレポするリンクをメッセージの末尾に追加します。。",
                    "*** Teleporter Plugin が必要です! ***");

                ImGui.Separator();

                if (ImGui.Button("Save & Close"))
                {
                    IsOpen = false;

                    AetheryteLinkInChatPlugin.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                    AetheryteLinkInChatPlugin.Instance.Logger.Information("Config saved");
                }

                ImGui.End();
            }
        }
    }
}
