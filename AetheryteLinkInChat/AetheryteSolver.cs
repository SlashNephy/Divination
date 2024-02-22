using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Data;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin.Services;
using Divination.AetheryteLinkInChat.Config;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public class AetheryteSolver
{
    private const uint MaxCalculateDepth = 5;
    private const double BetweenAreaDistanceCost = 8.0;

    private readonly ExcelSheet<Aetheryte> aetheryteSheet;
    private readonly ExcelSheet<Map> mapSheet;
    private readonly ExcelSheet<MapMarker> mapMarkerSheet;
    private readonly ExcelSheet<World> worldSheet;
    private readonly ExcelSheet<TerritoryType> territoryTypeSheet;

    public AetheryteSolver(IDataManager dataManager)
    {
        aetheryteSheet = dataManager.GetExcelSheet<Aetheryte>() ?? throw new ApplicationException("aetheryteSheet == null");
        mapSheet = dataManager.GetExcelSheet<Map>() ?? throw new ApplicationException("mapSheet == null");
        mapMarkerSheet = dataManager.GetExcelSheet<MapMarker>() ?? throw new ApplicationException("mapMarkerSheet == null");
        worldSheet = dataManager.GetExcelSheet<World>() ?? throw new ApplicationException("worldSheet == null");
        territoryTypeSheet = dataManager.GetExcelSheet<TerritoryType>() ?? throw new ApplicationException("territoryTypeSheet == null");
    }

    public IEnumerable<ITeleportPath> CalculateTeleportPathsForMapLink(MapLinkPayload payload)
    {
        var paths = CalculateTeleportPaths(payload.TerritoryType, payload.Map)
            .MinBy(paths =>
            {
                var distance = 0.0;
                var (x, y) = ((double)payload.XCoord, (double)payload.YCoord);

                foreach (var path in paths)
                {
                    var (markerX, markerY) = ConvertMarkerToCoordinate(path.Marker, path.Map);
                    DalamudLog.Log.Verbose("P1 = ({X1}, {Y1}), P2 = ({X2}, {Y2})", x, y, markerX, markerY);

                    DalamudLog.Log.Verbose("path = {S}", path);
                    if (path is AetheryteTeleportPath { Aetheryte.AethernetGroup: > 0 })
                    {
                        DalamudLog.Log.Verbose("skip distance calculation: this is aethernet: {S}", path);
                    }
                    else
                    {
                        distance += CalculateEuclideanDistance(x, y, markerX, markerY);
                    }

                    switch (path)
                    {
                        case AetheryteTeleportPath aetheryte:
                            (x, y) = ConvertMarkerToCoordinate(aetheryte.Marker, aetheryte.Map);
                            continue;
                        case BoundaryTeleportPath boundary:
                            (x, y) = ConvertMarkerToCoordinate(boundary.ConnectedMarker, boundary.ConnectedMap);
                            distance += BetweenAreaDistanceCost;
                            continue;
                    }
                }

                DalamudLog.Log.Verbose("distance = {D}, paths = {P}", distance, paths);
                return distance;
            });

        return paths?.Reverse() ?? Array.Empty<ITeleportPath>();
    }

    public void AppendGrandCompanyAetheryte(
        List<ITeleportPath> paths,
        uint grandCompanyAetheryteId,
        SeString message,
        World? currentWorld,
        ushort currentTerritoryTypeId)
    {
        var territory = territoryTypeSheet.GetRow(currentTerritoryTypeId);
        if (territory == default)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: territory == default");
            return;
        }

        var grandCompanyAetheryteIds = Enum.GetValues<GrandCompanyAetheryte>().Select(x => (uint)x).ToList();
        var aetheryte = GetAetherytesInTerritoryType(territory)
            .Select(x => x.aetheryte)
            .FirstOrDefault(x => grandCompanyAetheryteIds.Contains(x.RowId));
        if (aetheryte == default)
        {
            if (grandCompanyAetheryteId == default)
            {
                DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: grandCompanyAetheryteId == default");
                return;
            }

            aetheryte = aetheryteSheet.GetRow(grandCompanyAetheryteId);
            if (aetheryte == default)
            {
                DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: aetheryte == null");
                return;
            }
        }

        var text = string.Join(" ", message.Payloads.OfType<TextPayload>().Select(x => x.Text)).ToLower();
        var world = worldSheet.Where(x => x.IsPublic && x.DataCenter.Row == currentWorld?.DataCenter.Value?.RowId).FirstOrDefault(x => text.Contains(x.Name.RawString.ToLower()));
        if (world == default)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: world == null");
            return;
        }

        if (world.RowId == currentWorld?.RowId)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: world == currentWorld");
            return;
        }

        var (marker, map) = GetMarkerFromAetheryte(aetheryte);
        if (marker == default)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: marker == null");
            return;
        }
        if (map == default)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: map == null");
            return;
        }

        paths.Insert(0, new WorldTeleportPath(aetheryte, world, marker, map));
    }

    private IEnumerable<ITeleportPath[]> CalculateTeleportPaths(TerritoryType territoryType, Map map, uint depth = 0)
    {
        // 現在の探索深度が上限に達したら終了
        if (depth >= MaxCalculateDepth)
        {
            yield break;
        }

        // エリア内のエーテライトを探す
        foreach (var (aetheryte, marker) in GetAetherytesInTerritoryType(territoryType))
        {
            yield return new ITeleportPath[]
            {
                new AetheryteTeleportPath(aetheryte, marker, map),
            };
        }

        // エリア内のマップ境界を探す
        foreach (var marker in GetBoundariesInMap(map))
        {
            var connectedMap = mapSheet.GetRow(marker.DataKey);
            var connectedTerritoryType = connectedMap?.TerritoryType.Value;
            var connectedMarker = mapMarkerSheet
                // エリア境界のマーカー
                .Where(x => x.DataType == 1)
                // 近接エリアに移動した先のマーカーを探す
                .FirstOrDefault(x => x.RowId == connectedMap?.MapMarkerRange && x.DataKey == map.RowId);

            DalamudLog.Log.Verbose("marker = {S} ({N})", marker.PlaceNameSubtext.Value?.Name.RawString ?? "", marker.DataKey);
            DalamudLog.Log.Verbose("connectedTerritoryType = {S}", connectedTerritoryType?.PlaceName.Value?.Name.RawString ?? "");
            DalamudLog.Log.Verbose("connectedMap = {S}", connectedMap?.PlaceName.Value?.Name.RawString ?? "");
            DalamudLog.Log.Verbose("connectedMarker = {S}", connectedMarker?.PlaceNameSubtext.Value?.Name.RawString ?? "");

            if (connectedTerritoryType != default && connectedMap != default && connectedMarker != default)
            {
                foreach (var paths in CalculateTeleportPaths(connectedTerritoryType, connectedMap, ++depth))
                {
                    yield return paths.Prepend(new BoundaryTeleportPath(connectedMarker, connectedMap, marker, map)).ToArray();
                }
            }
        }
    }

    private IEnumerable<(Aetheryte aetheryte, MapMarker marker)> GetAetherytesInTerritoryType(TerritoryType territoryType)
    {
        foreach (var aetheryte in aetheryteSheet)
        {
            // 対象のエリア内に限定
            if (aetheryte.Territory.Row != territoryType.RowId)
            {
                continue;
            }

            // エーテライトのマップマーカーを探す
            var marker = mapMarkerSheet
                // エーテライトのマーカーに限定
                .Where(x => x.DataType is 3 or 4)
                .FirstOrDefault(x => x.DataKey == aetheryte.RowId);
            if (marker == default)
            {
                continue;
            }

            yield return (aetheryte, marker);
        }
    }

    private IEnumerable<MapMarker> GetBoundariesInMap(Map map)
    {
        return mapMarkerSheet
            // エリア境界のマーカー
            .Where(x => x.DataType == 1)
            // 現在のマップのマーカー
            .Where(x => x.RowId == map.MapMarkerRange);
    }

    private static (double, double) ConvertMarkerToCoordinate(MapMarker marker, Map map)
    {
        var x = marker.X * 42.0 / 2048 / map.SizeFactor * 100 + 1;
        var y = marker.Y * 42.0 / 2048 / map.SizeFactor * 100 + 1;
        return (x, y);
    }

    private static double CalculateEuclideanDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    private (MapMarker? marker, Map? map) GetMarkerFromAetheryte(Aetheryte aetheryte)
    {
        var marker = mapMarkerSheet.Where(x => x.DataType == 3).FirstOrDefault(x => x.DataKey == aetheryte.RowId);
        var map = mapSheet.FirstOrDefault(x => x.MapMarkerRange == marker?.RowId);
        return (marker, map);
    }

    public Aetheryte? GetAetheryteById(uint id)
    {
        return aetheryteSheet.GetRow(id);
    }
}
