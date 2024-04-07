using System;
using System.Collections.Concurrent;
using System.Linq;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using ImGuiNET;

namespace Divination.FaloopIntegration;

public class ActiveMobUi : IWindow
{
    private readonly ConcurrentDictionary<string, MobSpawnEvent> mobs = new();

    private bool isDrawing = true;
    public bool IsDrawing
    {
        get => isDrawing && !mobs.IsEmpty;
        set => isDrawing = value;
    }

    public void Draw()
    {
        if (!IsDrawing)
        {
            return;
        }

        if (ImGui.Begin(Localization.ActiveMob))
        {
            foreach (var mob in mobs.OrderBy(x => x.Value.SpawnedAt))
            {
                DrawMob(mob.Value);
            }

            ImGui.End();
        }
    }

    private static void DrawMob(MobSpawnEvent ev)
    {
        var span = DateTime.Now - ev.SpawnedAt;
        ImGui.Text(
            $"{Utils.GetRankIconChar(ev.Rank).ToIconString()} {ev.Mob.Singular.RawString}{SeIconChar.CrossWorld.ToIconString()}{ev.World.Name.RawString} {span:mm\\:ss}");
    }

    public void OnMobSpawn(MobSpawnEvent ev)
    {
        mobs[ev.Id] = ev;
    }

    public void OnMobDeath(MobDeathEvent ev)
    {
        mobs.TryRemove(ev.Id, out _);
    }
}
