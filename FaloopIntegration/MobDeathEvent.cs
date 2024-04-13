using System;
using Lumina.Excel.GeneratedSheets;

namespace Divination.FaloopIntegration;

public record MobDeathEvent(uint MobId, uint WorldId, int ZoneInstance, MobRank Rank, DateTime KilledAt)
{
    public BNpcName Mob => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<BNpcName>()?.GetRow(MobId) ?? throw new InvalidOperationException("invalid mob ID");
    public World World => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(WorldId) ?? throw new InvalidOperationException("invalid world ID");

    public string Id => $"{MobId}_{WorldId}_{ZoneInstance}";
}
