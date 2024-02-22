using System.Collections.Generic;

namespace Divination.DiscordIntegration.IpcModel;

public class UpdateTemplatesPayload
{
    public string? Source { get; init; }
    public string? Group { get; init; }
    public Dictionary<string, string?>? Templates { get; init; }
}
