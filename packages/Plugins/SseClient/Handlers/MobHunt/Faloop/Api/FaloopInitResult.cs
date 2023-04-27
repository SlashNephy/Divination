using System.Collections.Generic;
using Newtonsoft.Json;

#pragma warning disable 8618

namespace Divination.SseClient.Handlers.MobHunt.Faloop.Api;

public class FaloopInitResult
{
    [JsonProperty("success")] public bool Success { get; set; }

    [JsonProperty("mobs")] public List<Mob> Mobs { get; set; }

    [JsonProperty("zones")] public List<Zone> Zones { get; set; }

    [JsonProperty("territories")] public List<Territory> Territories { get; set; }

    [JsonProperty("dataCenters")] public List<DataCenter> DataCenters { get; set; }

    [JsonProperty("worlds")] public List<World> Worlds { get; set; }

    public class Mob
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("rank")] public string Rank { get; set; }

        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("zoneId")] public int ZoneId { get; set; }
    }

    public class Spawn
    {
        [JsonProperty("types")] public List<int> Types { get; set; }

        [JsonProperty("journey")] public List<int> Journey { get; set; }

        [JsonProperty("location")] public string Location { get; set; }
    }

    public class Zone
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("territoryId")] public int TerritoryId { get; set; }

        [JsonProperty("mapSize")] public int MapSize { get; set; }

        [JsonProperty("mapOffset")] public MapOffsetData MapOffset { get; set; }

        [JsonProperty("sizeFactor")] public int SizeFactor { get; set; }

        public class MapOffsetData
        {
            [JsonProperty("x")] public int X { get; set; }

            [JsonProperty("y")] public int Y { get; set; }
        }
    }

    public class Territory
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("key")] public string Key { get; set; }
    }

    public class DataCenter
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("key")] public string Key { get; set; }
    }

    public class World
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("dataCenterId")] public int DataCenterId { get; set; }

        [JsonProperty("status")] public int Status { get; set; }
    }
}