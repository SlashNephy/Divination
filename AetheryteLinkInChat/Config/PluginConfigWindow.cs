using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    private const int MinQueuedTeleportDelay = 250;
    private const int MaxQueuedTeleportDelay = 5000;

    private readonly string[] grandCompanyAetheryteNames;
    private readonly ImmutableList<Aetheryte> aetherytes;

    public PluginConfigWindow()
    {
        var sheet = AetheryteLinkInChat.Instance.Dalamud.DataManager.GetExcelSheet<Aetheryte>();
        if (sheet == default)
        {
            throw new InvalidOperationException("aetheryte sheet is null");
        }

        grandCompanyAetheryteNames = Enum.GetValues<GrandCompanyAetheryte>()
            .Select(x => sheet?.GetRow((uint)x)?.PlaceName.Value?.Name.RawString ?? Enum.GetName(x) ?? string.Empty)
            .ToArray();
        aetherytes = sheet.Where(x => x.IsAetheryte && !x.Invisible && x.RowId != 1).ToImmutableList();
    }

    public override void Draw()
    {
        if (ImGui.Begin(Localization.ConfigWindowTitle.Format(AetheryteLinkInChat.Instance.Name),
            ref IsOpen,
            ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.BeginTabBar("tabs"))
            {
                DrawGeneralTab();
                DrawAetheryteListTab();
                ImGui.EndTabBar();
            }

            ImGui.Separator();

            if (ImGui.Button(Localization.SaveConfigButton))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }

    private void DrawGeneralTab()
    {
        if (ImGui.BeginTabItem("General"))
        {
            ImGui.Checkbox(Localization.AllowTeleportQueueing, ref Config.AllowTeleportQueueing);
            if (Config.AllowTeleportQueueing)
            {
                ImGui.SliderInt(Localization.QueueTeleportDelay, ref Config.QueuedTeleportDelay, MinQueuedTeleportDelay, MaxQueuedTeleportDelay);
            }

            ImGui.Indent();
            ImGui.Text(Localization.QueuedTeleportDescription);
            ImGui.Unindent();

            ImGui.Spacing();

            ImGui.Combo(Localization.PreferredGrandCompanyAetheryte,
                ref Config.PreferredGrandCompanyAetheryte,
                grandCompanyAetheryteNames,
                grandCompanyAetheryteNames.Length);
            ImGui.Indent();
            ImGui.Text(Localization.PreferredGrandCompanyAetheryteDescription);
            ImGui.Unindent();

            ImGui.Checkbox(Localization.ConsiderTeleportsToOtherWorlds, ref Config.ConsiderTeleportsToOtherWorlds);
            ImGui.Indent();
            ImGui.Text(Localization.ConsiderTeleportsToOtherWorldsDescription);
            ImGui.Text(Localization.ConsiderTeleportsToOtherWorldsDisclaimer);
            ImGui.Unindent();

            ImGui.Checkbox(Localization.EnableLifestreamIntegration, ref Config.EnableLifestreamIntegration);
            ImGui.Indent();
            ImGui.Text(Localization.EnableLifestreamIntegrationDescription);
            ImGui.Unindent();

            ImGui.Checkbox(Localization.DisplayLineBreak, ref Config.DisplayLineBreak);

            // Maybe not the best location for this...
            if (Config.DisplayLineBreak)
                Config.DisplayLinkAtEnd = true;

            ImGui.BeginDisabled(Config.DisplayLineBreak);
            ImGui.Checkbox(Localization.DisplayLinkAtEnd, ref Config.DisplayLinkAtEnd);
            ImGui.EndDisabled();

            ImGui.Checkbox(Localization.EnableChatNotificationOnTeleport, ref Config.EnableChatNotificationOnTeleport);
            ImGui.Checkbox(Localization.EnableQuestNotificationOnTeleport, ref Config.EnableQuestNotificationOnTeleport);

            ImGui.EndTabItem();
        }
    }

    private void DrawAetheryteListTab()
    {
        if (ImGui.BeginTabItem("Aetheryte List"))
        {
            ImGui.Text(Localization.IgnoredAetherytes);

            if (ImGui.BeginTable("aethetytes", 3, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY, new Vector2(500f, 600f)))
            {
                ImGui.TableSetupColumn("ID");
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn(string.Empty);
                ImGui.TableHeadersRow();

                foreach (var aetheryte in aetherytes)
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text(aetheryte.RowId.ToString());

                    ImGui.TableNextColumn();
                    var aetheryteName = aetheryte.PlaceName.Value?.Name.RawString ?? "???";
                    var zoneName = aetheryte.Map.Value?.PlaceName.Value?.Name.RawString ?? "???";

                    if (Config.IgnoredAetheryteIds.Contains(aetheryte.RowId))
                    {
                        ImGui.TextDisabled($"{aetheryteName} ({zoneName})");

                        ImGui.TableNextColumn();
                        if (ImGui.Button($"Unignore##{aetheryte.RowId}"))
                        {
                            Config.IgnoredAetheryteIds.Remove(aetheryte.RowId);
                        }
                    }
                    else
                    {
                        ImGui.Text($"{aetheryteName} ({zoneName})");

                        ImGui.TableNextColumn();
                        if (ImGui.Button($"Ignore##{aetheryte.RowId}"))
                        {
                            Config.IgnoredAetheryteIds.Add(aetheryte.RowId);
                        }
                    }
                }

                ImGui.EndTable();
            }

            ImGui.EndTabItem();
        }
    }
}
