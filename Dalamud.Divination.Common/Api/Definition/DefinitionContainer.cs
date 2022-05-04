using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Definition
{
    /// <summary>
    ///     定義ファイルを表す JSON モデルクラスです。
    /// </summary>
    public abstract class DefinitionContainer
    {
        public string? Patch;

        public string? Version;
        [JsonIgnore]
        public bool IsObsolete { get; internal set; } = true;
    }
}
