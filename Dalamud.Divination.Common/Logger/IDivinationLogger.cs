using System;

namespace Dalamud.Divination.Common.Logger
{
    public interface IDivinationLogger : IDisposable
    {
        public string Name { get; }

        public void Append(LogLevel level, string message);

        public bool IsEnabledFor(LogLevel level);
    }
}
