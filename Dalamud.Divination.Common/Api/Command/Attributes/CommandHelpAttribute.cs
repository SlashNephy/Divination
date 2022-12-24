using System;

namespace Dalamud.Divination.Common.Api.Command.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CommandHelpAttribute : Attribute
{
    public CommandHelpAttribute(string help)
    {
        Help = help;
    }

    public string Help { get; }
}
