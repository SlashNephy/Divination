using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Version;

namespace Dalamud.Divination.Common.Api
{
    public interface IDivinationApi
    {
        public IVersionManager VersionManager { get; }

        public IChatClient ChatClient { get; }

        public ICommandProcessor? CommandProcessor { get; }

        public IBugReporter BugReporter { get; }
    }
}
