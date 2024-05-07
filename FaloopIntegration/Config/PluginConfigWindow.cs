using System;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using ImGuiNET;

namespace Divination.FaloopIntegration.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin(Localization.ConfigWindowTitle.Format(FaloopIntegration.Instance.Name), ref IsOpen))
        {
            DrawAccountConfig();
            DrawPerRankConfigs();
#if DEBUG
            DrawDebugConfig();
#endif

            if (ImGui.Button(Localization.SaveConfigButton))
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
        if (ImGui.CollapsingHeader(Localization.Account))
        {
            ImGui.Indent();

            ImGui.InputText(Localization.AccountUsername, ref Config.FaloopUsername, 32);
            ImGui.InputText(Localization.AccountPassword, ref Config.FaloopPassword, 128);

            ImGui.Unindent();
        }
    }

    private void DrawPerRankConfigs()
    {
        if (ImGui.CollapsingHeader(Localization.NotificationPreferences))
        {
            ImGui.Indent();

            if (ImGuiEx.CheckboxConfig(Localization.EnableActiveMobUi, ref FaloopIntegration.Instance.Config.EnableActiveMobUi))
            {
                FaloopIntegration.Instance.Ui.IsDrawing = FaloopIntegration.Instance.Config.EnableActiveMobUi;
            }
            ImGuiEx.CheckboxConfig(Localization.HideActiveMobUiInDuty, ref FaloopIntegration.Instance.Config.HideActiveMobUiInDuty);

            ImGui.Checkbox(Localization.EnableSimpleReports, ref FaloopIntegration.Instance.Config.EnableSimpleReports);

            DrawPerRankConfig(Localization.RankS, ref Config.RankS);
            DrawPerRankConfig(Localization.RankFate, ref Config.Fate);

            ImGui.Unindent();
        }
    }

    private void DrawPerRankConfig(string label, ref PluginConfig.PerRankConfig config)
    {
        if (ImGui.CollapsingHeader(label))
        {
            ImGui.Indent();

            ImGui.Combo($"{Localization.ReportChannel}##{label}", ref config.Channel, channels, channels.Length);

            if (ImGui.CollapsingHeader($"{Localization.ReportConditions}##{label}"))
            {
                ImGui.Indent();

                ImGui.Checkbox($"{Localization.EnableSpawnReport}##{label}", ref config.EnableSpawnReport);
                ImGui.SameLine();
                ImGui.Checkbox($"{Localization.EnableDeathReport}##{label}", ref config.EnableDeathReport);

                ImGui.Combo($"{Localization.ReportJurisdiction}##{label}", ref config.Jurisdiction, jurisdictions, jurisdictions.Length);
                ImGui.SameLine();
                ImGuiEx.HelpMarker(Localization.ReportJurisdictionDescription);
                if (config.Jurisdiction >= (int)Jurisdiction.Region)
                {
                    ImGui.Checkbox($"{Localization.IncludeOceaniaDataCenter}##{label}", ref config.IncludeOceaniaDataCenter);
                }

                ImGui.Text(Localization.ReportExpansions);
                ImGui.Indent();

                foreach (var expansion in gameExpansions)
                {
                    ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(config.Expansions, expansion, out _);
                    ImGui.Checkbox($"{Enum.GetName(expansion)}##{label}", ref value);
                    ImGui.SameLine();
                }

                ImGui.NewLine();
                ImGui.Unindent();

                if (ImGui.CollapsingHeader($"{Localization.IgnoreReports}##{label}"))
                {
                    ImGui.Indent();

                    ImGui.Checkbox($"{Localization.ReportIgnoreInDuty}##{label}", ref config.DisableInDuty);

                    ImGui.Checkbox($"{Localization.ReportIgnoreOrphanDeathReport}##{label}", ref config.SkipOrphanReport);
                    ImGui.SameLine();
                    ImGuiEx.HelpMarker(Localization.ReportIgnoreOrphanDeathReportDescription);

                    ImGui.Checkbox($"{Localization.ReportIgnorePendingReport}##{label}", ref config.SkipPendingReport);

                    ImGui.Unindent();
                }

                ImGui.Unindent();
            }

            ImGui.Unindent();
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
    private readonly GameExpansion[] gameExpansions = Enum.GetValues<GameExpansion>();
}
