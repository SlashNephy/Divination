using System;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using Dalamud.Logging;
using Divination.FaloopIntegration.Faloop.Model;
using ImGuiNET;

namespace Divination.FaloopIntegration.Config;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{FaloopIntegrationPlugin.Instance.Name} Config", ref IsOpen, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
        {
            if (ImGui.CollapsingHeader("Account"))
            {
                ImGui.InputText("Faloop Username", ref Config.FaloopUsername, 32);
                ImGui.InputText("Faloop Password", ref Config.FaloopPassword, 128);
            }

            if (ImGui.CollapsingHeader("Jurisdiction"))
            {
                ImGui.Combo("Rank S", ref Config.RankSJurisdiction, jurisdictions, jurisdictions.Length);
                ImGui.Combo("Rank A", ref Config.RankAJurisdiction, jurisdictions, jurisdictions.Length);
                ImGui.Combo("Rank B", ref Config.RankBJurisdiction, jurisdictions, jurisdictions.Length);
                ImGui.Combo("Fate", ref Config.FateJurisdiction, jurisdictions, jurisdictions.Length);
            }

            if (ImGui.CollapsingHeader("Report"))
            {
                ImGui.Combo("Channel", ref Config.Channel, channels, channels.Length);

                ImGui.Checkbox("Spawn", ref Config.EnableSpawnReport);
                ImGui.Checkbox("Death", ref Config.EnableDeathReport);
            }

            ImGui.Separator();

            if (ImGui.Button("Emit mock payload"))
            {
                Task.Run(async () =>
                {
                    try
                    {
                        FaloopIntegrationPlugin.Instance.OnMobReport(MockData.SpawnMobReport);
                        await Task.Delay(3000);
                        FaloopIntegrationPlugin.Instance.OnMobReport(MockData.DeathMobReport);
                    }
                    catch (Exception exception)
                    {
                        PluginLog.Error(exception, "mock");
                    }
                });
            }

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Interface.SavePluginConfig(Config);
                FaloopIntegrationPlugin.Instance.Connect();
            }

            ImGui.End();
        }
    }

    private readonly string[] jurisdictions = Enum.GetNames<Jurisdiction>();
    private readonly string[] channels = Enum.GetNames<XivChatType>();
}
