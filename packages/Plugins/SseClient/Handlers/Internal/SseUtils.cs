using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud.String;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Divination.SseClient.Payloads;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Divination.SseClient.Handlers
{
    public static class SseUtils
    {
        private static readonly Guid OriginId = Guid.NewGuid();

        public static bool IsSelfPayload(SsePayload payload)
        {
            return payload.OriginId == OriginId;
        }

        public static void SendPayload(string ev, SsePayload payload)
        {
            if (string.IsNullOrEmpty(SseClientPlugin.Instance.Config.EndpointUrl))
            {
                return;
            }

            Task.Run(async () =>
            {
                var player = SseClientPlugin.Instance.Dalamud.ClientState.LocalPlayer;
                payload.Origin = player?.Name.TextValue;
                payload.OriginId = OriginId;

                payload.TerritoryTypeId = SseClientPlugin.Instance.Dalamud.ClientState.TerritoryType;
                payload.WorldId = player?.CurrentWorld.Id;

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                await client.PostAsync($"{SseClientPlugin.Instance.Config.EndpointUrl}/collect/{ev}?token={SseClientPlugin.Instance.Config.Token}", content);
                PluginLog.Verbose($"Sent payload = {json}");
            });
        }

        private static readonly string CrossWorldIconString = new IconPayload(BitmapFontIcon.CrossWorld).ToUtf8String();

        public static string FormatName(SsePayload payload)
        {
            var (player, world) = ExtractPlayer(payload.SenderSeString);
            return world is null or "Valefor" ? player : $"{player}{CrossWorldIconString}{world}";
        }

        public static (string name, string? world) ExtractPlayer(SeString sender)
        {
            foreach (var payload in sender.Payloads)
            {
                switch (payload)
                {
                    case PlayerPayload playerPayload:
                        return (playerPayload.PlayerName, playerPayload.World.Name);
                    case TextPayload textPayload:
                        if (textPayload.Text.Contains((char) SeIconChar.CrossWorld))
                        {
                            var sliced = textPayload.Text.Split((char) SeIconChar.CrossWorld);
                            return (sliced[0], sliced[1]);
                        }
                        else
                        {
                            return (textPayload.Text, null);
                        }
                }
            }

            throw new AggregateException($"Unknown player format: {sender.TextValue}");
        }

        private static readonly IconPayload SseClientMessageIcon = new(BitmapFontIcon.None);
        private static readonly string ReceivedMessageIconString = new IconPayload(BitmapFontIcon.ElementalLevel).ToUtf8String();

        public static void PrintSseChat(XivChatEntry entry)
        {
            if (SseClientPlugin.Instance.Config.EnableDoNotDisturb)
            {
                return;
            }

            // クロスワールドアイコンを変換
            // TODO: Support Dalamud 4
            // entry.Name = entry.Name.TextValue.Replace($"{(char) SeIconChar.CrossWorld}", CrossWorldIconString);

            if (SseClientPlugin.Instance.Config.ShowIconOnReceivedMessages)
            {
                entry.Name = ReceivedMessageIconString + entry.Name;
            }

            // hack: メッセージ終端にマークを付与
            entry.Message = entry.Message.Append(SseClientMessageIcon);

            SseClientPlugin.Instance.Divination.Chat.EnqueueChat(entry);
        }

        public static bool IsSseMessage(SeString message)
        {
            if (message.Payloads.LastOrDefault() is IconPayload iconPayload && iconPayload.Icon == SseClientMessageIcon.Icon)
            {
                return message.Payloads.Remove(iconPayload);
            }

            return false;
        }
    }
}
