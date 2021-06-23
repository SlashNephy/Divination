using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Boilerplate
{
    public interface ICommandSupport : ICommandProvider
    {
        public string CommandPrefix { get; }
    }
}
