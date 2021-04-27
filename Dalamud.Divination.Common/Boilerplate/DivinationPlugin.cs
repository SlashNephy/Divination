using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationDalamudPlugin<TC> : IDisposable where TC : class, IPluginConfiguration
    {
        public abstract void Entry();

        public string Name { get; }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
        }
    }
}
