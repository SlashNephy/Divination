﻿using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Divination.FaloopIntegration.Faloop.Model;

public static class MockData
{
    private static readonly MobReportData.Spawn Spawn = new(DateTime.UtcNow, 1, 399, [643], null);

    public static readonly MobReportData SpawnMobReport = new("spawn",
        MobId: 4376,
        WorldId: 52,
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Spawn))!);

    private static readonly MobReportData.Death Death = new(1, StartedAt: Spawn.Timestamp.AddMinutes(3), PrevStartedAt: Spawn.Timestamp.AddDays(-7));

    public static readonly MobReportData DeathMobReport = new("death",
        MobId: 4376,
        WorldId: 52,
        ZoneInstance: 0,
        Data: JsonObject.Create(JsonSerializer.SerializeToElement(Death))!);
}
