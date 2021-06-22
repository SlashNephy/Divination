using System;

namespace Dalamud.Divination.Common.Api.Version
{
    public interface IVersionManager : IDisposable
    {
        public IGitVersion PluginVersion { get; }
        public IGitVersion LibraryVersion { get; }
    }
}
