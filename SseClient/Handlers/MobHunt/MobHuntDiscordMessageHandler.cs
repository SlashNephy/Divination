using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.MobHunt;

public class MobHuntDiscordMessageHandler : ISsePayloadReceiver
{
    public bool CanReceive(string eventId)
    {
        return eventId == "mobhunt_discord" && SseClient.Instance.Config.ReceiveMobHuntDiscordMessages;
    }

    public void Receive(string eventId, SsePayload payload)
    {
        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntDiscordMessagesType,
            Name = payload.Sender,
            Message = ParseRawMapText(payload.Message)
        });
    }

    private readonly Regex mapPattern = new(@"([^\s]+) \( (\d{2}\.\d)\s{1,2}, (\d{2}\.\d) \)", RegexOptions.Compiled);

    private SeString ParseRawMapText(string text)
    {
        var payloads = new List<Payload>();

        var rawText = text.Replace((char) SeIconChar.LinkMarker, ' ');
        var mapTexts = mapPattern.Matches(rawText);
        if (mapTexts.Count > 0)
        {
            foreach (Match match in mapTexts)
            {
                var placeName = match.Groups[1].Value;
                var x = float.Parse(match.Groups[2].Value);
                var y = float.Parse(match.Groups[3].Value);

                var result = rawText.Split(new[] { match.Value }, StringSplitOptions.None);
                if (result.Length != 2)
                {
                    continue;
                }

                var mapLink = SeString.CreateMapLink(placeName, x, y);
                if (mapLink == null)
                {
                    continue;
                }

                payloads.Add(new TextPayload(result[0]));
                payloads.AddRange(mapLink.Payloads);
                payloads.Add(new TextPayload(result[1]));
                rawText = result[1];
            }
        }

        if (payloads.Count == 0)
        {
            payloads.Add(new TextPayload(text));
        }

        return new SeString(payloads);
    }
}