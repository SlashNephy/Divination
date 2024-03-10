using System;
using System.Text.Json;
using Divination.FaloopIntegration.Faloop.Model;
using Lumina.Excel.GeneratedSheets;

namespace Divination.FaloopIntegration;

public record MobDeathEvent(MobReportData Data, BNpcName Mob, World World, string Rank)
{
    public readonly MobReportData.Death Death = Data.Data.Deserialize<MobReportData.Death>() ?? throw new InvalidOperationException("Death is null");
}
