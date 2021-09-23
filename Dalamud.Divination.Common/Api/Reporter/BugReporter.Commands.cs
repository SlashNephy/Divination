using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;

namespace Dalamud.Divination.Common.Api.Reporter
{
    internal sealed partial class BugReporter
    {
        public class Commands : ICommandProvider
        {
            private readonly IBugReporter reporter;

            public Commands(IBugReporter reporter)
            {
                this.reporter = reporter;
            }

            [Command("report", "<message...>")]
            [CommandHelp("<message> とともにログファイルや設定ファイルを開発者に送信します。")]
            private void OnReportCommand(CommandContext context)
            {
                Task.Run(async () =>
                {
                    var message = context.GetArgument("message");
                    await reporter.SendAsync(message);
                });
            }
        }
    }
}
