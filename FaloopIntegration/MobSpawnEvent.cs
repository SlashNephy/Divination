using System;
using System.Linq;
using System.Numerics;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

namespace Divination.FaloopIntegration;

public record MobSpawnEvent(
    uint MobId,
    uint WorldId,
    uint TerritoryTypeId,
    int ZoneInstance,
    MobRank Rank,
    DateTime SpawnedAt,
    string? Reporter,
    string? Location)
{
    [JsonIgnore]
    public BNpcName Mob => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<BNpcName>()?.GetRow(MobId) ?? throw new InvalidOperationException("invalid mob ID");
    [JsonIgnore]
    public World World => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(WorldId) ?? throw new InvalidOperationException("invalid world ID");
    [JsonIgnore]
    public TerritoryType TerritoryType => FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(TerritoryTypeId) ?? throw new InvalidOperationException("invalid territory type ID");
    [JsonIgnore]
    public Map Map => TerritoryType.Map.Value ?? throw new InvalidOperationException("invalid map ID");

    [JsonIgnore]
    public string Id => $"{MobId}_{WorldId}_{ZoneInstance}";

    [JsonIgnore]
    public Vector2? Coordinates
    {
        get
        {
            if (Location == default)
            {
                return default;
            }

            var n = 41 / (Map.SizeFactor / 100.0);
            var loc = Location.Split([','], 2)
                .Select(int.Parse)
                .Select(x => x / 2048.0 * n + 1)
                .Select(x => (float)x)
                .ToList();
            return new Vector2(loc[0], loc[1]);
        }
    }

    [JsonIgnore]
    public Vector3? WorldPosition
    {
        get
        {
            if (Location == default)
            {
                return default;
            }

            var loc = Location.Split([','], 2).Select(int.Parse).ToArray();
            var x = ((loc[0] - 1024f) * 100f / Map.SizeFactor) - Map.OffsetX;
            var y = ((loc[1] - 1024f) * 100f / Map.SizeFactor) - Map.OffsetY;
            return new Vector3(x, 0, y);
        }
    }
}
