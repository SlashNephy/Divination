using System;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using ImGuiNET;

namespace Divination.FaloopIntegration.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{FaloopIntegrationPlugin.Instance.Name} Config", ref IsOpen))
        {
            DrawAccountConfig();
            DrawPerRankConfigs();
            DrawDebugConfig();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
                FaloopIntegrationPlugin.Instance.Connect();
            }

            ImGui.End();
        }
    }

    private void DrawAccountConfig()
    {
        if (ImGui.CollapsingHeader("Account"))
        {
            ImGui.InputText("Faloop Username", ref Config.FaloopUsername, 32);
            ImGui.InputText("Faloop Password", ref Config.FaloopPassword, 128);
        }
    }

    private void DrawPerRankConfigs()
    {
        if (ImGui.CollapsingHeader("Per Rank"))
        {
            ImGui.Indent();

            DrawPerRankConfig("Rank S", ref Config.RankS);
            DrawPerRankConfig("Rank A", ref Config.RankA);
            DrawPerRankConfig("Rank B", ref Config.RankB);
            DrawPerRankConfig("Fate", ref Config.Fate);

            ImGui.Unindent();
        }
    }

    private void DrawPerRankConfig(string label, ref PluginConfig.PerRankConfig config)
    {
        if (ImGui.CollapsingHeader(label))
        {
            ImGui.Combo($"Channel##{label}", ref config.Channel, channels, channels.Length);
            ImGui.Combo($"Jurisdiction##{label}", ref config.Jurisdiction, jurisdictions, jurisdictions.Length);

            ImGui.Checkbox($"Spawn Report##{label}", ref config.EnableSpawnReport);
            ImGui.SameLine();
            ImGui.Checkbox($"Death Report##{label}", ref config.EnableDeathReport);

            ImGui.Checkbox($"Disable Report In Duty##{label}", ref config.DisableInDuty);
        }
    }

    private void DrawDebugConfig()
    {
        if (ImGui.CollapsingHeader("Debug"))
        {
            if (ImGui.Button("Emit mock payload"))
            {
                FaloopIntegrationPlugin.Instance.EmitMockData();
            }
        }
    }

    private readonly string[] jurisdictions = Enum.GetNames<Jurisdiction>();
    private readonly string[] channels = Enum.GetNames<XivChatType>();
}
