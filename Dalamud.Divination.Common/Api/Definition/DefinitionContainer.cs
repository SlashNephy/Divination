using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Definition
{
    /// <summary>
    /// 定義ファイルを表す JSON モデルクラスです。
    /// </summary>
    public abstract class DefinitionContainer
    {
        [JsonIgnore]
        public bool IsObsolete { get; internal set; } = true;

        public string? Version;
        public string? Patch;
    }
}
