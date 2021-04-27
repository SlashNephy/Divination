using System.Reflection;

namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * IDivinationPlugin の抽象実装を行います。
     */
    public abstract partial class DivinationPlugin<TC>
    {
        public abstract string Name { get; }
        public abstract Assembly Assembly { get; }
        public abstract void Load();
        public abstract void Unload();
        public abstract TC LoadConfig();
    }
}
