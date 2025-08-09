using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;

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
            .Select(x => sheet.HasRow((uint)x) ?
                sheet.GetRow((uint)x).PlaceName.Value.Name.ExtractText() :
                Enum.GetName(x) ?? string.Empty)
            .ToArray();
        aetherytes = sheet.Where(x => x is { IsAetheryte: true, Invisible: false } && x.RowId != 1).ToImmutableList();
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

            if (ImGui.Button(new(Localization.SaveConfigButton)))
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
            ImGui.Checkbox(new(Localization.AllowTeleportQueueing), ref Config.AllowTeleportQueueing);
            if (Config.AllowTeleportQueueing)
            {
                ImGui.SliderInt(new(Localization.QueueTeleportDelay), ref Config.QueuedTeleportDelay, MinQueuedTeleportDelay, MaxQueuedTeleportDelay);
            }

            ImGui.Indent();
            ImGui.Text(new(Localization.QueuedTeleportDescription));
            ImGui.Unindent();

            ImGui.Spacing();

            ImGui.Combo(new(Localization.PreferredGrandCompanyAetheryte),
                ref Config.PreferredGrandCompanyAetheryte,
                grandCompanyAetheryteNames,
                grandCompanyAetheryteNames.Length);
            ImGui.Indent();
            ImGui.Text(new(Localization.PreferredGrandCompanyAetheryteDescription));
            ImGui.Unindent();

            ImGui.Checkbox(new(Localization.ConsiderTeleportsToOtherWorlds), ref Config.ConsiderTeleportsToOtherWorlds);
            ImGui.Indent();
            ImGui.Text(new(Localization.ConsiderTeleportsToOtherWorldsDescription));
            ImGui.Text(new(Localization.ConsiderTeleportsToOtherWorldsDisclaimer));
            ImGui.Unindent();

            ImGui.Checkbox(new(Localization.EnableLifestreamIntegration), ref Config.EnableLifestreamIntegration);
            ImGui.Indent();
            ImGui.Text(new(Localization.EnableLifestreamIntegrationDescription));
            ImGui.Unindent();

            ImGui.Checkbox(new(Localization.DisplayLineBreak), ref Config.DisplayLineBreak);

            // Maybe not the best location for this...
            if (Config.DisplayLineBreak)
                Config.DisplayLinkAtEnd = true;

            ImGui.BeginDisabled(Config.DisplayLineBreak);
            ImGui.Checkbox(new(Localization.DisplayLinkAtEnd), ref Config.DisplayLinkAtEnd);
            ImGui.EndDisabled();

            ImGui.Checkbox(new(Localization.EnableChatNotificationOnTeleport), ref Config.EnableChatNotificationOnTeleport);
            ImGui.Checkbox(new(Localization.EnableQuestNotificationOnTeleport), ref Config.EnableQuestNotificationOnTeleport);

            ImGui.EndTabItem();
        }
    }

    private void DrawAetheryteListTab()
    {
        if (ImGui.BeginTabItem("Aetheryte List"))
        {
            ImGui.Text(new(Localization.IgnoredAetherytes));

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
                    var aetheryteName = "???";
                    var zoneName = "???";

                    if (aetheryte.PlaceName.IsValid)
                    {
                        aetheryteName = aetheryte.PlaceName.Value.Name.ExtractText();
                    }

                    if (aetheryte.Map is { IsValid: true, Value.PlaceName.IsValid: true })
                    {
                        zoneName = aetheryte.Map.Value.PlaceName.Value.Name.ExtractText();
                    }

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
