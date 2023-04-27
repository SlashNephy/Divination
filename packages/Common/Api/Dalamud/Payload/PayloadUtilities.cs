using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Dalamud.Payload;

public static class PayloadUtilities
{
    public static IEnumerable<Game.Text.SeStringHandling.Payload> HighlightAngleBrackets(string? text)
    {
        if (text == null)
        {
            yield break;
        }

        var bracketRegex = new Regex(@"(<.+?>)");
        var matches = bracketRegex.Matches(text);
        if (matches.Count > 0)
        {
            var original = text;
            var lastIndex = 0;

            foreach (Match match in matches)
            {
                yield return new TextPayload(text[..(match.Index - lastIndex)]);
                yield return new UIForegroundPayload(500);
                yield return new TextPayload(match.Value);
                yield return UIForegroundPayload.UIForegroundOff;

                lastIndex = match.Index + match.Length;
                text = original[lastIndex..];
            }
        }

        yield return new TextPayload(text);
    }
}
