using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Data;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public class AetheryteSolver
{
    private const uint MaxCalculateDepth = 5;
    private const double BetweenAreaDistanceCost = 10.0;

    private readonly ExcelSheet<Aetheryte> aetheryteSheet;
    private readonly ExcelSheet<Map> mapSheet;
    private readonly ExcelSheet<MapMarker> mapMarkerSheet;

    public AetheryteSolver(DataManager dataManager)
    {
        aetheryteSheet = dataManager.GetExcelSheet<Aetheryte>() ?? throw new ApplicationException("aetheryteSheet == null");
        mapSheet = dataManager.GetExcelSheet<Map>() ?? throw new ApplicationException("mapSheet == null");
        mapMarkerSheet = dataManager.GetExcelSheet<MapMarker>() ?? throw new ApplicationException("mapMarkerSheet == null");
    }

    public IEnumerable<ITeleportPath> CalculateTeleportPathsForMapLink(MapLinkPayload payload)
    {
        var paths = CalculateTeleportPaths(payload.TerritoryType, payload.Map)
            .MinBy(paths =>
            {
                var distance = 0.0;
                var (x, y) = ((double) payload.XCoord, (double) payload.YCoord);

                foreach (var path in paths)
                {
                    var (markerX, markerY) = ConvertMarkerToCoordinate(path.Marker, path.Map);
                    PluginLog.Verbose("P1 = ({X1}, {Y1}), P2 = ({X2}, {Y2})", x, y, markerX, markerY);
                    distance += CalculateEuclideanDistance(x, y, markerX, markerY);

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

                PluginLog.Verbose("distance = {D}, paths = {P}", distance, paths);
                return distance;
            });

        return paths?.Reverse() ?? new ITeleportPath[] { };
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

            PluginLog.Verbose("marker = {S} ({N})", marker.PlaceNameSubtext.Value?.Name.RawString ?? "", marker.DataKey);
            PluginLog.Verbose("connectedTerritoryType = {S}", connectedTerritoryType?.PlaceName.Value?.Name.RawString ?? "");
            PluginLog.Verbose("connectedMap = {S}", connectedMap?.PlaceName.Value?.Name.RawString ?? "");
            PluginLog.Verbose("connectedMarker = {S}", connectedMarker?.PlaceNameSubtext.Value?.Name.RawString ?? "");

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

    private static (double, double) ConvertCoordinateToRaw(double x, double y, Map map)
    {
        var x2= (x - 1) * 2048 * map.SizeFactor / 42.0 / 100;
        var y2 = (y - 1) * 2048 * map.SizeFactor / 42.0 / 100;
        return (x2, y2);
    }

    private static double CalculateEuclideanDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    public Aetheryte? FindAetheryteByName(string name)
    {
        return aetheryteSheet.FirstOrDefault(x => x.PlaceName.Value?.Name.RawString == name);
    }
}
