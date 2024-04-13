using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using ImGuiNET;

namespace Divination.FaloopIntegration;

public class ActiveMobUi : IWindow, IDisposable
{
    private readonly ConcurrentDictionary<string, MobSpawnEvent> mobs = new();
    private readonly Task cleanupTask;
    private readonly CancellationTokenSource cancellation = new();

    public ActiveMobUi()
    {
        cleanupTask = new Task(CleanUp);
        cleanupTask.Start();
    }

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

        if (ImGui.Begin(Localization.ActiveMob, ImGuiWindowFlags.AlwaysAutoResize))
        {
            foreach (var mob in mobs.Values.OrderBy(x => x.SpawnedAt))
            {
                DrawMob(mob);
            }

            ImGui.End();
        }
    }

    private static void DrawMob(MobSpawnEvent ev)
    {
        var span = DateTime.UtcNow - ev.SpawnedAt;
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

    private async void CleanUp()
    {
        while (!cancellation.IsCancellationRequested)
        {
            foreach (var mob in mobs.Values.Where(x => DateTime.UtcNow - x.SpawnedAt > GetMaxAge(x)))
            {
                mobs.TryRemove(mob.Id, out _);
            }

            await Task.Delay(10 * 1000);
        }
    }

    private static TimeSpan GetMaxAge(MobSpawnEvent ev)
    {
        return ev.Rank switch
        {
            MobRank.S => TimeSpan.FromMinutes(10),
            MobRank.SS => TimeSpan.FromMinutes(10),
            MobRank.FATE => TimeSpan.FromMinutes(30),
            _ => throw new ArgumentOutOfRangeException(nameof(ev.Rank), ev.Rank, null),
        };
    }

    public void Dispose()
    {
        cancellation.Cancel();
    }
}
