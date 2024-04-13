using System;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

namespace Divination.FaloopIntegration;

public record MobSpawnEvent(
    uint MobId,
    uint WorldId,
    uint TerritoryTypeId,
    int ZoneInstance,
    int? ZoneLocationId,
    MobRank Rank,
    DateTime SpawnedAt,
    string? Reporter)
{
    [JsonIgnore]
    public BNpcName Mob => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<BNpcName>()?.GetRow(MobId) ?? throw new InvalidOperationException("invalid mob ID");
    [JsonIgnore]
    public World World => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(WorldId) ?? throw new InvalidOperationException("invalid world ID");
    [JsonIgnore]
    public TerritoryType TerritoryType => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(TerritoryTypeId) ?? throw new InvalidOperationException("invalid territory type ID");

    [JsonIgnore]
    public string Id => $"{MobId}_{WorldId}_{ZoneInstance}";
}
