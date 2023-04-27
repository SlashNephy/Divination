using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    private const int MinQueuedTeleportDelay = 250;
    private const int MaxQueuedTeleportDelay = 5000;

    private readonly string[] grandCompanyAetheryteNames;

    public PluginConfigWindow()
    {
        var sheet = AetheryteLinkInChatPlugin.Instance.Dalamud.DataManager.GetExcelSheet<Aetheryte>();
        grandCompanyAetheryteNames = Enum.GetValues<GrandCompanyAetheryte>()
            .Select(x => sheet?.GetRow((uint)x)?.PlaceName.Value?.Name.RawString ?? Enum.GetName(x) ?? string.Empty)
            .ToArray();
    }

    public override void Draw()
    {
        if (ImGui.Begin(Localization.ConfigWindowTitle.Format(AetheryteLinkInChatPlugin.Instance.Name), ref IsOpen, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize))
        {
            ImGui.Checkbox(Localization.AllowTeleportQueueing.ToString(), ref Config.AllowTeleportQueueing);
            if (Config.AllowTeleportQueueing)
            {
                ImGui.SliderInt(Localization.QueueTeleportDelay.ToString(), ref Config.QueuedTeleportDelay,
                    MinQueuedTeleportDelay,
                    MaxQueuedTeleportDelay);
            }

            ImGui.Indent();
            ImGui.Text(Localization.QueuedTeleportDescription.ToString());
            ImGui.Unindent();

            ImGui.Spacing();

            ImGui.Combo(Localization.PreferredGrandCompanyAetheryte.ToString(),
                ref Config.PreferredGrandCompanyAetheryte,
                grandCompanyAetheryteNames,
                grandCompanyAetheryteNames.Length);
            ImGui.Indent();
            ImGui.Text(Localization.PreferredGrandCompanyAetheryteDescription.ToString());
            ImGui.Unindent();

            ImGui.Checkbox(Localization.ConsiderTeleportsToOtherWorlds.ToString(),
                ref Config.ConsiderTeleportsToOtherWorlds);
            ImGui.Indent();
            ImGui.Text(Localization.ConsiderTeleportsToOtherWorldsDescription.ToString());
            ImGui.Text(Localization.ConsiderTeleportsToOtherWorldsDisclaimer.ToString());
            ImGui.Unindent();

            ImGui.Separator();

            if (ImGui.Button(Localization.SaveConfigButton.ToString()))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
            }

            ImGui.End();
        }
    }
}
