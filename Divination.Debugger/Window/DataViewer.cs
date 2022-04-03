using System;
using System.Linq;
using ImGuiNET;

namespace Divination.Debugger.Window;

public class DataViewer
{
    private readonly DataType dataType;
    private readonly byte[] data;
    private readonly bool isEnableFilter;
    private readonly long filter;

    public DataViewer(DataType dataType, byte[] data, bool isEnableFilter, long filter)
    {
        this.dataType = dataType;
        this.data = data;
        this.isEnableFilter = isEnableFilter;
        this.filter = filter;
    }

    public void Draw()
    {
        switch (dataType)
        {
            case DataType.UInt8:
                Draw(data, 1);
                break;
            case DataType.Int8:
                Draw(data.TransformIntoInt8(), 1);
                break;
            case DataType.UInt16:
                Draw(data.TransformIntoUInt16(), 2);
                break;
            case DataType.Int16:
                Draw(data.TransformIntoInt16(), 2);
                break;
            case DataType.UInt32:
                Draw(data.TransformIntoUInt32(), 4);
                break;
            case DataType.Int32:
                Draw(data.TransformIntoInt32(), 4);
                break;
            case DataType.UInt64:
                Draw(data.TransformIntoUInt64(), 8);
                break;
            case DataType.Int64:
                Draw(data.TransformIntoInt64(), 8);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Draw<T>(T[] source, int byteCount) where T : struct
    {
        if (!IsMatchedPre(source))
        {
            return;
        }

        ImGui.Columns(2);

        ImGui.Text("Index"); ImGui.NextColumn();
        ImGui.Text($"Value ({Enum.GetName(typeof(DataType), dataType)})"); ImGui.NextColumn();
        ImGui.Separator();

        foreach (var (index, value) in source.Where(IsMatchedPost).Select((x, i) => (i, x)))
        {
            ImGui.Text($"0x{index * byteCount:X4}  ({index * byteCount})"); ImGui.NextColumn();
            ImGui.Text($"{value}"); ImGui.NextColumn();
        }

        ImGui.Columns(1);
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
