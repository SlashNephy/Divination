using System;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using ImGuiNET;

namespace Divination.FaloopIntegration.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{FaloopIntegration.Instance.Name} Config", ref IsOpen))
        {
            DrawAccountConfig();
            DrawPerRankConfigs();
            DrawDebugConfig();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
                FaloopIntegration.Instance.Connect();
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

            ImGui.Text("Expansions");
            ImGui.Indent();
            foreach (var patchVersion in Enum.GetValues<MajorPatch>())
            {
                ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(config.MajorPatches, patchVersion, out _);
                ImGui.Checkbox(Enum.GetName(patchVersion) + $"##{label}", ref value);
                ImGui.SameLine();
            }
            ImGui.Unindent();
            ImGui.NewLine();

            ImGui.Checkbox($"Spawn Report##{label}", ref config.EnableSpawnReport);
            ImGui.SameLine();
            ImGui.Checkbox($"Death Report##{label}", ref config.EnableDeathReport);

            ImGui.Checkbox($"Disable Report In Duty##{label}", ref config.DisableInDuty);

            ImGui.Checkbox($"Skip Orphan Report##{label}", ref config.SkipOrphanReport);
        }
    }

    private static void DrawDebugConfig()
    {
        if (ImGui.CollapsingHeader("Debug"))
        {
            if (ImGui.Button("Emit mock payload"))
            {
                FaloopIntegration.Instance.EmitMockData();
            }
        }
    }

    private readonly string[] jurisdictions = Enum.GetNames<Jurisdiction>();
    private readonly string[] channels = Enum.GetNames<XivChatType>();
    private readonly string[] majorPatches = Enum.GetNames<MajorPatch>();
}
