using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Api.Config
{
    public sealed class EmptyConfig : IPluginConfiguration
    {
        public int Version { get; set; }
    }
}
