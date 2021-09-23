using System;
using System.Linq;

namespace Dalamud.Divination.Common.Api.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CommandAttribute : Attribute
    {
        public string[] Commands { get; }

        public CommandAttribute(string command, params string[] subCommands)
        {
            Commands = subCommands.Prepend(command).ToArray();
        }
    }
}
