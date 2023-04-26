namespace Dalamud.Divination.Common.Api.Version;

internal partial class VersionManager : IVersionManager
{
    public VersionManager(IGitVersion pluginVersion, IGitVersion libraryVersion)
    {
        Plugin = pluginVersion;
        Divination = libraryVersion;
    }

    public IGitVersion Plugin { get; }
    public IGitVersion Divination { get; }
}
