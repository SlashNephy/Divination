using Dalamud.Configuration;

namespace Divination.Template;

public class PluginConfig : IPluginConfiguration
{
    public bool SomeFlag;

    public int Version { get; set; }
}
