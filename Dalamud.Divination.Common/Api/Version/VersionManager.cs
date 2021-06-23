using Dalamud.Divination.Common.Api.Logger;

namespace Dalamud.Divination.Common.Api.Version
{
    internal partial class VersionManager : IVersionManager
    {
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug("VersionManager");

        public IGitVersion PluginVersion { get; }
        public IGitVersion LibraryVersion { get; }

        public VersionManager(IGitVersion pluginVersion, IGitVersion libraryVersion)
        {
            PluginVersion = pluginVersion;
            LibraryVersion = libraryVersion;
        }

        public void Dispose()
        {
            logger.Dispose();
        }
    }
}
