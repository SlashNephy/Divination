using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace Dalamud.Divination.Common.Api.Dalamud;

public sealed class DalamudLog
{
    [PluginService] public static IPluginLog Log { get; private set; } = null!;
}
