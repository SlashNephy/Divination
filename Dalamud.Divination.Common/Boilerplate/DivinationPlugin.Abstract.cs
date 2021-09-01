using System.Reflection;

namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * IDivinationPlugin の抽象実装を行います。
     */
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        public virtual string Name => _instance!.GetType().Name;
        public virtual Assembly Assembly => Assembly.GetExecutingAssembly();

        public virtual void DisposeManaged()
        {
        }

        public virtual void DisposeUnmanaged()
        {
        }
    }
}
