using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Divination.FaloopIntegration.Faloop.Model;

public static class MockData
{
    private static readonly MobReportData.Spawn Spawn = new(DateTime.Parse("2022-12-09T12:06:33.031Z"), 1, 399, [643], null);

    public static readonly MobReportData SpawnMobReport = new("spawn",
        MobId: 4376,
        WorldId: 52,
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Spawn))!);

    private static readonly MobReportData.Death Death = new(1,
        StartedAt: DateTime.Parse("2022-12-09T12:09:38.718Z"),
        PrevStartedAt: DateTime.Parse("2022-12-05T02:28:12.931Z"));

    public static readonly MobReportData DeathMobReport = new("death",
        MobId: 4376,
        WorldId: 52,
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Death))!);
}
