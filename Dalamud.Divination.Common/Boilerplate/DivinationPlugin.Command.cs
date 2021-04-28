using System.Collections.Generic;
using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        private readonly List<DivinationCommand> commands = new();
    }
}
