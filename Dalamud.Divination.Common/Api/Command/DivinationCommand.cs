using System.Reflection;

namespace Dalamud.Divination.Common.Api.Command
{
    internal class DivinationCommand
    {
        public CommandAttribute Attribute { get; }
        public MethodInfo Method { get; }

        public DivinationCommand(CommandAttribute attribute, MethodInfo method)
        {
            Attribute = attribute;
            Method = method;
        }
    }
}
