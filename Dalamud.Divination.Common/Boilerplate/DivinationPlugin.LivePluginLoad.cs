using System.Diagnostics.CodeAnalysis;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public string? AssemblyLocation { get; private set; }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void SetLocation(string dllPath)
        {
            AssemblyLocation = dllPath;
        }
    }
}
