using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Data;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public class AetheryteSolver
{
    private readonly ExcelSheet<Aetheryte> aetheryteSheet;
    private readonly ExcelSheet<MapMarker> mapMarkerSheet;

    public AetheryteSolver(DataManager dataManager)
    {
        aetheryteSheet = dataManager.GetExcelSheet<Aetheryte>() ?? throw new ApplicationException("aetheryteSheet == null");
        mapMarkerSheet = dataManager.GetExcelSheet<MapMarker>() ?? throw new ApplicationException("mapMarkerSheet == null");
    }

    public record TeleportPath(Aetheryte? Aetheryte, MapMarker Marker);

    public IEnumerable<TeleportPath> CalculateTeleportPathsForMapLink(MapLinkPayload payload)
    {
        var aetherytes = GetAetherytesInTerritoryType(payload.TerritoryType).ToList();
        if (aetherytes.Count > 0)
        {
            var nearest = aetherytes.MinBy(x =>
                CalculateEuclideanDistanceBetweenMarker(payload.XCoord, payload.YCoord, x.marker, payload.Map));
            if (nearest != default)
            {
                yield return new TeleportPath(nearest.aetheryte, nearest.marker);
                yield break;
            }
        }

        var boundaries = GetBoundariesInMap(payload.Map).ToList();
        if (boundaries.Count > 0)
        {

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
            .Where(x => x.RowId == map.RowId);
    }

    private static (double, double) ConvertMarkerToCoordinate(MapMarker marker, Map map)
    {
        var x = marker.X * 42.0 / 2048 / map.SizeFactor * 100 + 1;
        var y = marker.Y * 42.0 / 2048 / map.SizeFactor * 100 + 1;
        return (x, y);
    }

    private static double CalculateEuclideanDistanceBetweenMarker(float x, float y, MapMarker marker, Map map)
    {
        var (markerX, markerY) = ConvertMarkerToCoordinate(marker, map);
        return CalculateEuclideanDistance(x, y, markerX, markerY);
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
