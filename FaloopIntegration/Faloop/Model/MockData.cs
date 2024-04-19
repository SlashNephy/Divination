using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Divination.FaloopIntegration.Faloop.Model;

public static class MockData
{
    private static readonly MobReportData.Spawn Spawn = new(DateTime.UtcNow, "the_dravanian_hinterlands", [643], null);

    public static readonly MobReportData SpawnMobReport = new(MobReportActions.Spawn,
        Ids: new(
            MobId: "the_pale_rider",
            WorldId: "valefor"
        ),
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Spawn))!);

    private static readonly MobReportData.Death Death = new(Spawn.Timestamp.AddMinutes(3));

    public static readonly MobReportData DeathMobReport = new(MobReportActions.Death,
        Ids: new(
            MobId: "the_pale_rider",
            WorldId: "valefor"
        ),
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Death))!);
}
