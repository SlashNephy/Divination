using System;
using System.Text.Json.Serialization;
using Dalamud.Configuration;
using Divination.Debugger.Window;

namespace Divination.Debugger;

public class PluginConfig : IPluginConfiguration
{
    public bool OpenAtStart;
    public bool EnableVerboseChatLog;

    public int PlayerDataTypeIndex;
    public bool PlayerEnableValueFilter;
    public long PlayerFilterValue;

    public bool NetworkEnableListener;
    public bool NetworkListenDownload = true;
    public bool NetworkListenUpload = true;
    public int NetworkDataTypeIndex;
    public bool NetworkEnableValueFilter;
    public long NetworkFilterValue;
    public bool NetworkEnableOpcodeFilter;
    public int NetworkFilterOpcode;
    public bool NetworkLogMatchedPackets = true;

    [JsonIgnore] private readonly Array dataTypes = Enum.GetValues(typeof(DataType));

    [JsonIgnore]
    public DataType PlayerDataType => (DataType)dataTypes.GetValue(PlayerDataTypeIndex)!;

    [JsonIgnore]
    public DataType NetworkDataType => (DataType)dataTypes.GetValue(NetworkDataTypeIndex)!;

    public int Version { get; set; }
}
