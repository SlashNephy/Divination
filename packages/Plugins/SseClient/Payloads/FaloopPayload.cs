using System;
using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable 8618

namespace Divination.SseClient.Payloads
{
    public class FaloopPayload
    {
        public string Type { get; set; }
        public string? SubType { get; set; }
        public JToken? Data { get; set; }

        public class MobStatus
        {
            public int MobId { get; set; }
            public int WorldId { get; set; }
            public int? ZoneInstance { get; set; }
            public SpawnStatus? Spawn { get; set; }
            public SightingInfo? Sightings { get; set; }
            public WindowStatus? Window { get; set; }

            [JsonIgnore]
            public BNpcName Mob => SseClientPlugin.Instance.Dalamud.DataManager
                .GetExcelSheet<BNpcName>()?
                .GetRow((uint) MobId) ?? throw new ArgumentException($"MobId = {MobId} はサポートされていません。");
            [JsonIgnore]
            public World World => SseClientPlugin.Instance.Dalamud.DataManager
                .GetExcelSheet<World>()?
                .GetRow((uint) WorldId) ?? throw new ArgumentException($"WorldId = {WorldId} はサポートされていません。");

            public class SpawnStatus
            {
                public string?  Location { get; set; }
                public long Timestamp { get; set; }
                public int ReporterId { get; set; }
                public string ReporterName { get; set; }
            }

            public class SightingInfo
            {
                public int MobId { get; set; }
                public int WorldId { get; set; }
                public Dictionary<string, SightingPoint>? Sightings { get; set; }
            }
            public class SightingPoint
            {
                public long SightedAt { get; set; }
                public string? ReporterName { get; set; }
                public int? ReporterId { get; set; }
            }

            public class WindowStatus
            {
                public bool IsPostMaint { get; set; }
                public long StartedAt { get; set; }
            }
        }
    }
}
