using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using Dalamud.Bindings.ImGui;

namespace Divination.FaloopIntegration.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    private readonly Jurisdiction[] jurisdictions = Enum.GetValues<Jurisdiction>();
    private readonly string[] jurisdictionLabels;
    private readonly string[] channelLabels = Enum.GetNames<XivChatType>();
    private readonly GameExpansion[] gameExpansions = Enum.GetValues<GameExpansion>().Reverse().ToArray();
    private readonly string[] gameExpansionLabels;

    public PluginConfigWindow() : base()
    {
        jurisdictionLabels = jurisdictions.Select(jurisdiction =>
        {
            string label = jurisdiction switch
            {
                Jurisdiction.None => Localization.JurisdictionNone,
                Jurisdiction.World => Localization.JurisdictionWorld,
                Jurisdiction.DataCenter => Localization.JurisdictionDataCenter,
                Jurisdiction.Region => Localization.JurisdictionRegion,
                Jurisdiction.Travelable => Localization.JurisdictionTravelable,
                Jurisdiction.All => Localization.JurisdictionAll,
                _ => throw new ArgumentOutOfRangeException(nameof(jurisdiction), jurisdiction, "unknown Jurisdiction")
            };
            return label;
        }).ToArray();

        gameExpansionLabels = gameExpansions.Select(expansion =>
        {
            string label = expansion switch
            {
                GameExpansion.ARelmReborn => Localization.GameExpansionARelmReborn,
                GameExpansion.Heavensward => Localization.GameExpansionHeavensward,
                GameExpansion.Stormblood => Localization.GameExpansionStormblood,
                GameExpansion.Shadowbringers => Localization.GameExpansionShadowbringers,
                GameExpansion.Endwalker => Localization.GameExpansionEndwalker,
                GameExpansion.Dawntrail => Localization.GameExpansionDawntrail,
                _ => throw new ArgumentOutOfRangeException(nameof(expansion), expansion, "unknown GameExpansion")
            };
            return label;
        }).ToArray();
    }

    public override void Draw()
    {
        if (ImGui.Begin(Localization.ConfigWindowTitle.Format(FaloopIntegration.Instance.Name), ref IsOpen))
        {
            if (ImGui.BeginTabBar("configuration"))
            {
                if (ImGui.BeginTabItem(new(Localization.GeneralTab)))
                {
                    DrawGeneralTab();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(new(Localization.RankSTab)))
                {
                    DrawPerRankTab("rank_s", ref Config.RankS);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem(new(Localization.FateTab)))
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

            ImGui.NewLine();

            if (ImGui.Button(new(Localization.SaveConfigButton)))
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
        ImGui.Text(Localization.PluginStatus.Format(FaloopIntegration.Instance.Status switch
        {
            PluginStatus.NotReady => Localization.PluginStatusNotReady,
            PluginStatus.Connected => Localization.PluginStatusConnected,
            PluginStatus.Disconnected => Localization.PluginStatusDisconnected,
            _ => throw new ArgumentOutOfRangeException()
        }));

        if (ImGui.CollapsingHeader(new(Localization.Account)))
        {
            ImGui.Indent();

            ImGui.InputText(new(Localization.AccountUsername), ref Config.FaloopUsername, 32);
            ImGui.InputText(new(Localization.AccountPassword), ref Config.FaloopPassword, 128, ImGuiInputTextFlags.Password);

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

        ImGui.Checkbox(new(Localization.EnableSimpleReports), ref FaloopIntegration.Instance.Config.EnableSimpleReports);
    }

    private void DrawPerRankTab(string id, ref PluginConfig.PerRankConfig config)
    {
        ImGui.Combo($"{Localization.ReportChannel}##{id}", ref config.Channel, channelLabels, channelLabels.Length);

        ImGui.Checkbox($"{Localization.EnableSpawnReport}##{id}", ref config.EnableSpawnReport);
        ImGui.SameLine();
        ImGui.Checkbox($"{Localization.EnableDeathReport}##{id}", ref config.EnableDeathReport);

        ImGui.Text(new(Localization.ReportJurisdiction));
        ImGui.SameLine();
        ImGuiEx.HelpMarker(Localization.ReportJurisdictionDescription);
        ImGui.Indent();

        foreach (var (i, expansion) in gameExpansions.Select((x, i) => (i, x)))
        {
            ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(config.Jurisdictions, expansion, out _);
            var index = Array.IndexOf(jurisdictions, value);
            if (ImGui.Combo($"{gameExpansionLabels[i]}##{id}", ref index, jurisdictionLabels, jurisdictionLabels.Length))
            {
                value = jurisdictions[index];
            }
        }

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
}
