using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Command;

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

            [Command("Report", "message", Help = "<message?> とともにログファイルや設定ファイルを開発者に送信します。", Strict = false)]
            private void OnReportCommand(CommandContext context)
            {
                Task.Run(async () =>
                {
                    await reporter.SendAsync(context.ArgumentText);
                });
            }
        }
    }
}
