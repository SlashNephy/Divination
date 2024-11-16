using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Services;
using Divination.AetheryteLinkInChat.Config;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace Divination.AetheryteLinkInChat.Solver;

public class AetheryteSolver(IDataManager dataManager)
{
    private const uint MaxCalculateDepth = 5;
    private const double BetweenAreaDistanceCost = 8.0;

    private readonly ExcelSheet<Aetheryte> aetheryteSheet =
        dataManager.GetExcelSheet<Aetheryte>() ?? throw new ApplicationException("aetheryteSheet == null");
    private readonly ExcelSheet<Map> mapSheet = dataManager.GetExcelSheet<Map>() ?? throw new ApplicationException("mapSheet == null");
    private readonly SubrowExcelSheet<MapMarker> mapMarkerSheet =
        dataManager.GetSubrowExcelSheet<MapMarker>() ?? throw new ApplicationException("mapMarkerSheet == null");
    private readonly ExcelSheet<World> worldSheet = dataManager.GetExcelSheet<World>() ?? throw new ApplicationException("worldSheet == null");
    private readonly ExcelSheet<TerritoryType> territoryTypeSheet =
        dataManager.GetExcelSheet<TerritoryType>() ?? throw new ApplicationException("territoryTypeSheet == null");

    public IEnumerable<ITeleportPath> CalculateTeleportPathsForMapLink(MapLinkPayload payload)
    {
        var paths = CalculateTeleportPaths(payload.TerritoryType.Value, payload.Map.Value)
            .MinBy(paths =>
            {
                var distance = 0.0;
                var (x, y) = ((double)payload.XCoord, (double)payload.YCoord);

                foreach (var path in paths)
                {
                    var (markerX, markerY) = ConvertMarkerToCoordinate(path.Marker, path.Map);
                    DalamudLog.Log.Verbose("P1 = ({X1}, {Y1}), P2 = ({X2}, {Y2})", x, y, markerX, markerY);

                    DalamudLog.Log.Verbose("path = {S}", path);
                    if (path is AetheryteTeleportPath {Aetheryte.AethernetGroup: > 0})
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

        return paths?.Reverse() ?? [];
    }

    public void AppendGrandCompanyAetheryte(List<ITeleportPath> paths,
        uint grandCompanyAetheryteId,
        SeString message,
        World? currentWorld,
        ushort currentTerritoryTypeId)
    {
        var isTerritoryTypeFound = territoryTypeSheet.TryGetRow(currentTerritoryTypeId, out var territory);
        if (!isTerritoryTypeFound)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: TryGetRow failed");
            return;
        }

        var grandCompanyAetheryteIds = Enum.GetValues<GrandCompanyAetheryte>().Select(x => (uint)x).ToList();
        Aetheryte aetheryte;
        var isAetheryteFound = GetAetherytesInTerritoryType(territory)
            .Select(x => x.aetheryte)
            .TryGetFirst(x => grandCompanyAetheryteIds.Contains(x.RowId), out aetheryte);

        if (!isAetheryteFound)
        {
            if (grandCompanyAetheryteId == default)
            {
                DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: grandCompanyAetheryteId == default");
                return;
            }

            if (!aetheryteSheet.TryGetRow(grandCompanyAetheryteId, out aetheryte))
            {
                DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: aetheryte == null");
                return;
            }
        }

        World? world = DetectWorld(message, currentWorld);
        if (!world.HasValue)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: world == null");
            return;
        }

        if (world.Value.RowId == currentWorld?.RowId)
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: world == currentWorld");
            return;
        }

        if (!TryGetMarkerFromAetheryte(aetheryte, out var marker, out var map))
        {
            DalamudLog.Log.Debug("AppendGrandCompanyAetheryte: marker == null | map == null");
            return;
        }

        paths.Insert(0, new WorldTeleportPath(aetheryte, world.Value, marker, map));
    }

    public World? DetectWorld(SeString message, World? currentWorld)
    {
        var text = string.Join(" ", message.Payloads.OfType<TextPayload>().Select(x => x.Text));

        // trim texts within MapLinkPayload
        const string linkPattern = ".*?\\)";
        var rgx = new Regex(linkPattern);
        text = rgx.Replace(text, "");
        // replace Boxed letters with alphabets
        text = string.Join(string.Empty, text.Select(ReplaceSeIconChar));

        return worldSheet.Where(x => x.IsPublic)
            .TryGetFirst(x => text.Contains(x.Name.ExtractText(), StringComparison.OrdinalIgnoreCase), out var world)
            ? world
            : currentWorld;
    }

    private static char ReplaceSeIconChar(char c)
    {
        return c switch
        {
            (char)SeIconChar.BoxedLetterA => 'A',
            (char)SeIconChar.BoxedLetterB => 'B',
            (char)SeIconChar.BoxedLetterC => 'C',
            (char)SeIconChar.BoxedLetterD => 'D',
            (char)SeIconChar.BoxedLetterE => 'E',
            (char)SeIconChar.BoxedLetterF => 'F',
            (char)SeIconChar.BoxedLetterG => 'G',
            (char)SeIconChar.BoxedLetterH => 'H',
            (char)SeIconChar.BoxedLetterI => 'I',
            (char)SeIconChar.BoxedLetterJ => 'J',
            (char)SeIconChar.BoxedLetterK => 'K',
            (char)SeIconChar.BoxedLetterL => 'L',
            (char)SeIconChar.BoxedLetterM => 'M',
            (char)SeIconChar.BoxedLetterN => 'N',
            (char)SeIconChar.BoxedLetterO => 'O',
            (char)SeIconChar.BoxedLetterP => 'P',
            (char)SeIconChar.BoxedLetterQ => 'Q',
            (char)SeIconChar.BoxedLetterR => 'R',
            (char)SeIconChar.BoxedLetterS => 'S',
            (char)SeIconChar.BoxedLetterT => 'T',
            (char)SeIconChar.BoxedLetterU => 'U',
            (char)SeIconChar.BoxedLetterV => 'V',
            (char)SeIconChar.BoxedLetterW => 'W',
            (char)SeIconChar.BoxedLetterX => 'X',
            (char)SeIconChar.BoxedLetterY => 'Y',
            (char)SeIconChar.BoxedLetterZ => 'Z',
            _ => c,
        };
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
            yield return
            [
                new AetheryteTeleportPath(aetheryte, marker, map),
            ];
        }

        // エリア内のマップ境界を探す
        foreach (var marker in GetBoundariesInMap(map))
        {
            var connectedMap = mapSheet.GetRow(marker.DataKey.RowId);
            var connectedTerritoryType = connectedMap.TerritoryType.Value;
            var isMapMarkerFound = mapMarkerSheet
                // エリア境界のマーカー
                .SelectMany(x => x)
                .Where(x => x.DataType == 1)
                // 近接エリアに移動した先のマーカーを探す
                .TryGetFirst(x => x.RowId == connectedMap.MapMarkerRange && x.DataKey.RowId == map.RowId, out var connectedMarker);

            DalamudLog.Log.Verbose("marker = {S} ({N})", marker.PlaceNameSubtext.Value.Name.ExtractText(), marker.DataKey);
            DalamudLog.Log.Verbose("connectedTerritoryType = {S}", connectedTerritoryType.PlaceName.Value.Name.ExtractText());
            DalamudLog.Log.Verbose("connectedMap = {S}", connectedMap.PlaceName.Value.Name.ExtractText());
            DalamudLog.Log.Verbose("connectedMarker = {S}", isMapMarkerFound ? connectedMarker.PlaceNameSubtext.Value.Name.ExtractText() : "");

            if (!isMapMarkerFound)
                continue;

            foreach (var paths in CalculateTeleportPaths(connectedTerritoryType, connectedMap, ++depth))
            {
                yield return paths.Prepend(new BoundaryTeleportPath(connectedMarker, connectedMap, marker, map)).ToArray();
            }
        }
    }

    private IEnumerable<(Aetheryte aetheryte, MapMarker marker)> GetAetherytesInTerritoryType(TerritoryType territoryType)
    {
        foreach (var aetheryte in aetheryteSheet)
        {
            // 対象のエリア内に限定
            if (aetheryte.Territory.RowId != territoryType.RowId)
            {
                continue;
            }

            // skip invisible aetherytes
            if (aetheryte.Invisible)
            {
                continue;
            }

            // skip ignored aetherytes
            if (AetheryteLinkInChat.Instance.Config.IgnoredAetheryteIds.Contains(aetheryte.RowId))
            {
                continue;
            }

            // エーテライトのマップマーカーを探す
            var isMapMarkerFound = mapMarkerSheet
                // エーテライトのマーカーに限定
                .SelectMany(x => x)
                .Where(x => x.DataType is 3 or 4)
                .TryGetFirst(x => x.DataKey.RowId == aetheryte.RowId, out var marker);
            if (!isMapMarkerFound)
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
            .SelectMany(x => x)
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

    private bool TryGetMarkerFromAetheryte(Aetheryte aetheryte, out MapMarker resultMarker, out Map resultMap)
    {
        resultMarker = default;
        resultMap = default;

        var isMapMarkerFound = mapMarkerSheet
            .SelectMany(x => x)
            .Where(x => x.DataType == 3)
            .TryGetFirst(x => x.DataKey.RowId == aetheryte.RowId, out var marker);

        if (!isMapMarkerFound)
            return false;

        var isMapFound = mapSheet.TryGetFirst(x => x.MapMarkerRange == marker.RowId, out var map);

        resultMarker = marker;
        resultMap = map;
        return isMapFound;
    }

    public Aetheryte? GetAetheryteById(uint id)
    {
        return aetheryteSheet.GetRowOrDefault(id);
    }
}
