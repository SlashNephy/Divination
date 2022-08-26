using System;

namespace Dalamud.Divination.Common.Api.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HiddenCommandAttribute : Attribute
    {
        public bool HideInHelp { get; set; } = true;
        public bool HideInStartUp { get; set; } = true;
    }
}
