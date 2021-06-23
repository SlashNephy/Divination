using System.Reflection;

namespace Dalamud.Divination.Common.Api.Command
{
    public sealed class DivinationCommand
    {
        public CommandAttribute Attribute { get; }
        public MethodInfo Method { get; }
        internal readonly object Instance;

        public DivinationCommand(CommandAttribute attribute, MethodInfo method, object instance)
        {
            Attribute = attribute;
            Method = method;
            Instance = instance;
        }
    }
}
