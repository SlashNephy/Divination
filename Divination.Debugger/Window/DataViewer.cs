using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;

namespace Divination.Debugger.Window;

public class DataViewer
{
    private readonly bool isEnableFilter;
    private readonly long filter;
    private readonly List<DataRow>? rows;

    public DataViewer(DataType dataType, byte[] data, bool isEnableFilter, long filter)
    {
        this.isEnableFilter = isEnableFilter;
        this.filter = filter;

        rows = dataType switch
        {
            DataType.UInt8 => PrepareData(data, 1),
            DataType.Int8 => PrepareData(data.TransformIntoInt8(), 1),
            DataType.UInt16 => PrepareData(data.TransformIntoUInt16(), 2),
            DataType.Int16 => PrepareData(data.TransformIntoInt16(), 2),
            DataType.UInt32 => PrepareData(data.TransformIntoUInt32(), 4),
            DataType.Int32 => PrepareData(data.TransformIntoInt32(), 4),
            DataType.UInt64 => PrepareData(data.TransformIntoUInt64(), 8),
            DataType.Int64 => PrepareData(data.TransformIntoInt64(), 8),
            _ => throw new NotImplementedException(),
        };
    }

    public void Draw()
    {
        if (rows == null)
        {
            return;
        }

        ImGui.Columns(2);

        ImGui.Text("Index"); ImGui.NextColumn();
        ImGui.Text("Value"); ImGui.NextColumn();
        ImGui.Separator();

        foreach (var (index, value) in rows)
        {
            ImGui.Text($"0x{index:X4}  ({index})"); ImGui.NextColumn();
            ImGui.Text(value); ImGui.NextColumn();
        }

        ImGui.Columns(1);
    }

    public bool Any() => rows is { Count: > 0 };

    private List<DataRow>? PrepareData<T>(T[] source, int byteCount) where T : struct
    {
        if (!IsMatchedPre(source))
        {
            return null;
        }

        return source.Where(IsMatchedPost)
            .Select((x, i) => new DataRow(i * byteCount, x.ToString()!))
            .ToList();
    }

    private bool IsMatchedPre<T>(T[] source) where T : struct
    {
        if (!isEnableFilter)
        {
            return true;
        }

        switch (source)
        {
            case byte[]:
                return filter is >= byte.MinValue and <= byte.MaxValue;
            case sbyte[]:
                return filter is >= sbyte.MinValue and <= sbyte.MaxValue;
            case ushort[]:
                return filter is >= ushort.MinValue and <= ushort.MaxValue;
            case short[]:
                return filter is >= short.MinValue and <= short.MaxValue;
            case uint[]:
                return filter is >= uint.MinValue and <= uint.MaxValue;
            case int[]:
                return filter is >= int.MinValue and <= int.MaxValue;
            case ulong[]:
                return filter >= 0;
            case long[]:
                return true;
            default:
                throw new NotImplementedException();
        }
    }

    private bool IsMatchedPost<T>(T value) where T : struct
    {
        if (!isEnableFilter)
        {
            return true;
        }

        switch (value)
        {
            case byte u8:
                return u8 == filter;
            case sbyte i8:
                return i8 == filter;
            case ushort u16:
                return u16 == filter;
            case short i16:
                return i16 == filter;
            case uint u32:
                return u32 == filter;
            case int i32:
                return i32 == filter;
            case ulong u64:
                return u64 == (ulong) filter;
            case long i64:
                return i64 == filter;
            default:
                throw new NotImplementedException();
        }
    }
}
