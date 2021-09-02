using System;

namespace Dalamud.Divination.Common.Api.Version
{
    public interface IVersionManager : IDisposable
    {
        public IGitVersion Plugin { get; }
        public IGitVersion Divination { get; }
    }
}
