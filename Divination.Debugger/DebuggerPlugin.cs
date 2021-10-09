using System.Text;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Debugger
{
    public partial class DebuggerPlugin : DivinationPlugin<DebuggerPlugin, PluginConfig>,
        IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>
    {
        /*
        private static readonly Lazy<AntiDebug> AntiDebug = new Lazy<AntiDebug>(() => new AntiDebug(Interface.TargetModuleScanner));
        private static LogEventLevel _logEventLevel;
        */

        public DebuggerPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Divination.ConfigWindow!.IsDrawing = true;
            Dalamud.ChatGui.ChatMessage += OnChatMessage;

            /*
             AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .First(x => x.FullName == "Dalamud.Service" && x.GenericTypeArguments.First().FullName == "")
            _logEventLevel = DalamudLoggingLevelSwitch.MinimumLevel;
            DalamudLoggingLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
            AntiDebug.Value.Enable();
            */
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (Config.EnableVerboseChatLog)
            {
                var text = new StringBuilder();
                text.AppendLine($"[{type}, {isHandled}] {sender.TextValue} ({senderId}): {message.TextValue}");

                foreach (var payload in sender.Payloads)
                {
                    text.AppendLine($"  {payload}");
                }

                foreach (var payload in message.Payloads)
                {
                    text.AppendLine($"    {payload}");
                }

                PluginLog.Verbose("{Chat}", text.ToString());
            }
        }

        protected override void ReleaseManaged()
        {
            Dalamud.ChatGui.ChatMessage -= OnChatMessage;
        }

        public string MainCommandPrefix => "/debug";
        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();
    }
}
