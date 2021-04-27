using Dalamud.Divination.Common.Api;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TC>
    {
#pragma warning disable 8618
        public GitVersion Version { get; private set; }
#pragma warning restore 8618

        public GitVersion CommonVersion { get; } = new(System.Reflection.Assembly.GetExecutingAssembly());
    }
}
