using System.Collections.Generic;

namespace Divination.DiscordIntegration.IpcModel;

public class UpdateVariablesPayload
{
    public string? Source { get; init; }
    public string? Group { get; init; }
    public Dictionary<string, object?>? Variables { get; init; }
}