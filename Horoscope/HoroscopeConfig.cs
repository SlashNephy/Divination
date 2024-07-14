using System.Collections.Generic;
using Dalamud.Configuration;

namespace Divination.Horoscope;

public class HoroscopeConfig : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public Dictionary<string, bool> ModuleStates = new();
}
