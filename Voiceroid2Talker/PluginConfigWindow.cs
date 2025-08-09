using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Bindings.ImGui;

namespace Divination.Voiceroid2Talker;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{Voiceroid2Talker.Instance.Name} Config", ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGuiEx.CheckboxConfig("FC チャットを読み上げる",
                ref Config.EnableTtsFcChatOnInactive,
                "[FC] チャットを非アクティブ状態のときに読み上げます。",
                "*** SimpleVoiceroid2Proxy (https://github.com/SlashNephy/SimpleVoiceroid2Proxy) が必要です! ***");

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;

                Voiceroid2Talker.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                DalamudLog.Log.Information("Config saved");
            }

            ImGui.End();
        }
    }
}
