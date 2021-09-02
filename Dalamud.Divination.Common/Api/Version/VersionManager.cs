using Dalamud.Divination.Common.Api.Logger;

namespace Dalamud.Divination.Common.Api.Version
{
    internal partial class VersionManager : IVersionManager
    {
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug("VersionManager");

        public IGitVersion Plugin { get; }
        public IGitVersion Divination { get; }

        public VersionManager(IGitVersion pluginVersion, IGitVersion libraryVersion)
        {
            Plugin = pluginVersion;
            Divination = libraryVersion;
        }

        public void Dispose()
        {
            logger.Dispose();
        }
    }
}
