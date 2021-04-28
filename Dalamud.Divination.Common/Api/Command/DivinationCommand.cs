using System.Reflection;

namespace Dalamud.Divination.Common.Api.Command
{
    public class DivinationCommand
    {
        public CommandAttribute Command { get; }
        public MethodInfo Method { get; }

        public DivinationCommand(CommandAttribute command, MethodInfo method)
        {
            Command = command;
            Method = method;
        }
    }
}
