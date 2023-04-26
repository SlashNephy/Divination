using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Boilerplate.Features;

public interface ICommandSupport : ICommandProvider
{
    public string MainCommandPrefix { get; }
}
