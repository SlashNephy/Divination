using System.Text.RegularExpressions;

namespace Dalamud.Divination.Common.Api.Command
{
    public class CommandProcessor
    {
        private static readonly Regex CommandRegex = new(@"^そのコマンドはありません。： (?<command>.+)$", RegexOptions.Compiled);
        private static readonly Regex BracketRegex = new(@"(<.+?>)", RegexOptions.Compiled);
    }
}
