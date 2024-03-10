using System;
using System.Text.Json;
using Divination.FaloopIntegration.Faloop.Model;
using Lumina.Excel.GeneratedSheets;

namespace Divination.FaloopIntegration;

public record MobSpawnEvent(MobReportData Data, BNpcName Mob, World World, string Rank)
{
    public readonly MobReportData.Spawn Spawn = Data.Data.Deserialize<MobReportData.Spawn>() ?? throw new InvalidOperationException("Spawn is null");
}
