using System;
using Dalamud.Divination.Common.Api.Command;

namespace Divination.Debugger
{
    public partial class DebuggerPlugin
    {
        [Command("/cc", Help = "Debug Console をクリアします。")]
        private static void OnClearConsoleCommand()
        {
            Console.Clear();
        }
    }
}
