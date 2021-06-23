using System.Reflection;

namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * IDivinationPlugin の抽象実装を行います。
     */
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        public abstract string Name { get; }
        public abstract Assembly Assembly { get; }

        public abstract void Load();

        public virtual void DisposeManaged()
        {
        }

        public virtual void DisposeUnmanaged()
        {
        }
    }
}
