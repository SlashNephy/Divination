using System;
using Dalamud.Divination.Common.Api.Command.Attributes;

namespace Divination.Debugger
{
    public partial class DebuggerPlugin
    {
        [Command("/cc")]
        [CommandHelp("Debug Console をクリアします。")]
        private static void OnClearConsoleCommand()
        {
            Console.Clear();
        }
    }
}
