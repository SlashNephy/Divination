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
            if (ImGui.BeginTabBar("configuration"))
            {
                if (ImGui.BeginTabItem("General"))
                {
                    DrawGeneralTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(Localization.RankS))
                {
                    DrawPerRankTab("rank_s", ref Config.RankS);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(Localization.RankFate))
                {
                    DrawPerRankTab("fate", ref Config.Fate);
                    ImGui.EndTabItem();
                }

#if DEBUG
                if (ImGui.BeginTabItem("Debug"))
                {
                    DrawDebugTab();
                    ImGui.EndTabItem();
                }
#endif

                ImGui.EndTabBar();
            }

            if (ImGui.Button(Localization.SaveConfigButton))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
                FaloopIntegration.Instance.Connect();
            }

            ImGui.End();
        }
    }

    private void DrawGeneralTab()
    {
        ImGui.Text("Status: connected");

        if (ImGui.CollapsingHeader(Localization.Account))
        {
            ImGui.Indent();

            ImGui.InputText(Localization.AccountUsername, ref Config.FaloopUsername, 32);
            ImGui.InputText(Localization.AccountPassword, ref Config.FaloopPassword, 128, ImGuiInputTextFlags.Password);

            ImGui.Unindent();
        }

        if (ImGuiEx.CheckboxConfig(Localization.EnableActiveMobUi, ref FaloopIntegration.Instance.Config.EnableActiveMobUi))
        {
            FaloopIntegration.Instance.Ui.IsDrawing = FaloopIntegration.Instance.Config.EnableActiveMobUi;
        }
        if (FaloopIntegration.Instance.Config.EnableActiveMobUi)
        {
            ImGuiEx.CheckboxConfig(Localization.HideActiveMobUiInDuty, ref FaloopIntegration.Instance.Config.HideActiveMobUiInDuty);
        }

        ImGui.Checkbox(Localization.EnableSimpleReports, ref FaloopIntegration.Instance.Config.EnableSimpleReports);
    }

    private void DrawPerRankTab(string id, ref PluginConfig.PerRankConfig config)
    {
        ImGui.Combo($"{Localization.ReportChannel}##{id}", ref config.Channel, channels, channels.Length);

        ImGui.Checkbox($"{Localization.EnableSpawnReport}##{id}", ref config.EnableSpawnReport);
        ImGui.SameLine();
        ImGui.Checkbox($"{Localization.EnableDeathReport}##{id}", ref config.EnableDeathReport);

        ImGui.Combo($"{Localization.ReportJurisdiction}##{id}", ref config.Jurisdiction, jurisdictions, jurisdictions.Length);
        ImGui.SameLine();
        ImGuiEx.HelpMarker(Localization.ReportJurisdictionDescription);
        if (config.Jurisdiction >= (int)Jurisdiction.Region)
        {
            ImGui.Checkbox($"{Localization.IncludeOceaniaDataCenter}##{id}", ref config.IncludeOceaniaDataCenter);
        }

        ImGui.Text(Localization.ReportExpansions);
        ImGui.Indent();

        foreach (var expansion in gameExpansions)
        {
            ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(config.Expansions, expansion, out _);
            ImGui.Checkbox($"{Enum.GetName(expansion)}##{id}", ref value);
            ImGui.SameLine();
        }

        ImGui.NewLine();
        ImGui.Unindent();

        if (ImGui.CollapsingHeader($"{Localization.IgnoreReports}##{id}"))
        {
            ImGui.Indent();

            ImGui.Checkbox($"{Localization.ReportIgnoreInDuty}##{id}", ref config.DisableInDuty);

            ImGui.Checkbox($"{Localization.ReportIgnoreOrphanDeathReport}##{id}", ref config.SkipOrphanReport);
            ImGui.SameLine();
            ImGuiEx.HelpMarker(Localization.ReportIgnoreOrphanDeathReportDescription);

            ImGui.Checkbox($"{Localization.ReportIgnorePendingReport}##{id}", ref config.SkipPendingReport);

            ImGui.Unindent();
        }
    }

    private static void DrawDebugTab()
    {
        if (ImGui.Button("Emit mock payload"))
        {
            FaloopIntegration.Instance.EmitMockData();
        }
    }

    private readonly string[] jurisdictions = Enum.GetNames<Jurisdiction>();
    private readonly string[] channels = Enum.GetNames<XivChatType>();
    private readonly GameExpansion[] gameExpansions = Enum.GetValues<GameExpansion>();
}
