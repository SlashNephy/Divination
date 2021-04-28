using Dalamud.Divination.Common.Api;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
#pragma warning disable 8618
        public GitVersion Version => new(Assembly);
#pragma warning restore 8618

        public GitVersion CommonVersion => new(System.Reflection.Assembly.GetExecutingAssembly());
    }
}
