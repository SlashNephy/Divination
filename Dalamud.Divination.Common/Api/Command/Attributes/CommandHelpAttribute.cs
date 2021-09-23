using System;

namespace Dalamud.Divination.Common.Api.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandHelpAttribute : Attribute
    {
        public string Help { get; }

        public CommandHelpAttribute(string help)
        {
            Help = help;
        }
    }
}
