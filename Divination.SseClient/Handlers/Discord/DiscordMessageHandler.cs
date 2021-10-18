using System;
using System.Linq;
using System.Text;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.Discord
{
    public class DiscordMessageHandler : ISsePayloadReceiver, ISsePayloadEmitter
    {
        public bool CanReceive(string eventId)
        {
            return eventId == "discord_message" && SseClientPlugin.Instance.Config.ReceiveDiscordMessages;
        }

        public void Receive(string eventId, SsePayload payload)
        {
            SseUtils.PrintSseChat(new XivChatEntry
            {
                Message = payload.MessageSeString,
                Name = payload.SenderSeString,
                Type = SseClientPlugin.Instance.Config.DiscordMessagesType
            });
        }

        public bool CanEmit(XivChatType chatType) => SseClientPlugin.Instance.Config.SendFcMessages;

        public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
        {
            SseUtils.SendPayload("fc_chat", new SsePayload
            {
                ChatType = XivChatType.FreeCompany,
                Sender = $"[FC] {SseUtils.ExtractPlayer(sender).name}",
                Message = RenderDiscordMessage(message)
            });
        }

        // ReSharper disable once CognitiveComplexity
        private static string RenderDiscordMessage(SeString message)
        {
            var builder = new StringBuilder();
            var ignoring = false;

            foreach (var payload in message.Payloads)
            {
                switch (payload)
                {
                    case AutoTranslatePayload autoTranslatePayload:
                        var text = autoTranslatePayload.Text
                            .TrimStart((char) SeIconChar.AutoTranslateOpen)
                            .TrimEnd((char) SeIconChar.AutoTranslateClose)
                            .Trim();
                        builder.Append($"<:prefix:648211864609619976> {text} <:suffix:648211864785911828>");

                        break;
                    case EmphasisItalicPayload:
                        builder.Append('*');
                        break;
                    case QuestPayload questPayload:
                        builder.Append($":checkered_flag: {questPayload.Quest.Name} ({questPayload.Quest.PlaceName.Value.Name})");
                        break;
                    case ItemPayload itemPayload:
                        builder.Append(
                            $":baggage_claim: [{itemPayload.Item.Name}{(itemPayload.IsHQ ? " <:hq:603802451895910410>" : "")}]");
                        ignoring = true;

                        break;
                    case MapLinkPayload mapLinkPayload:
                        builder.Append(
                            $":triangular_flag_on_post: [{mapLinkPayload.PlaceName} {mapLinkPayload.CoordinateString}]");
                        ignoring = true;

                        break;
                    case PlayerPayload playerPayload:
                        builder.Append(
                            $":mens: [{playerPayload.PlayerName} :globe_with_meridians: {playerPayload.World.Name}]");
                        ignoring = true;

                        break;
                    case StatusPayload statusPayload:
                        builder.Append($":eight_spoked_asterisk: [{statusPayload.Status.Name}]");
                        ignoring = true;

                        break;
                    case TextPayload textPayload:
                        if (!ignoring)
                        {
                            var result = Enum.GetValues(typeof(SeIconChar)).Cast<SeIconChar>().Aggregate(textPayload.Text, (s, c) =>
                            {
                                var emoji = SeIconCharToEmoji(c);
                                return emoji == null ? s : s.Replace($"{(char) c}", emoji);
                            });

                            builder.Append(result);
                        }

                        break;
                    case RawPayload rawPayload:
                        if (rawPayload.Data.SequenceEqual(RawPayload.LinkTerminator.Data))
                        {
                            ignoring = false;
                        }

                        break;
                }
            }

            return builder.ToString();
        }

        // ReSharper disable once CognitiveComplexity
        private static string? SeIconCharToEmoji(SeIconChar icon)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (icon)
            {
                case SeIconChar.BotanistSprout:
                    return "<:novice:728859083679203329>";
                case SeIconChar.ItemLevel:
                    return "IL";
                case SeIconChar.LevelEn:
                case SeIconChar.LevelDe:
                case SeIconChar.LevelFr:
                    return "Lv";
                case SeIconChar.AutoTranslateOpen:
                    return "<:prefix:648211864609619976>";
                case SeIconChar.AutoTranslateClose:
                    return "<:suffix:648211864785911828>";
                case SeIconChar.HighQuality:
                    return "<:hq:603802451895910410>";
                case SeIconChar.Clock:
                    break;
                case SeIconChar.Gil:
                    break;
                case SeIconChar.Hyadelyn:
                    break;
                case SeIconChar.ArrowRight:
                    return ":arrow_right:";
                case SeIconChar.ArrowDown:
                    return ":arrow_down:";
                case SeIconChar.Number0:
                    return ":zero:";
                case SeIconChar.Number1:
                    return ":one:";
                case SeIconChar.Number2:
                    return ":two:";
                case SeIconChar.Number3:
                    return ":three:";
                case SeIconChar.Number4:
                    return ":four:";
                case SeIconChar.Number5:
                    return ":five:";
                case SeIconChar.Number6:
                    return ":six:";
                case SeIconChar.Number7:
                    return ":seven:";
                case SeIconChar.Number8:
                    return ":eight:";
                case SeIconChar.Number9:
                    return ":nine:";
                case SeIconChar.BoxedNumber0:
                    return ":zero:";
                case SeIconChar.BoxedNumber1:
                    return ":one:";
                case SeIconChar.BoxedNumber2:
                    return ":two:";
                case SeIconChar.BoxedNumber3:
                    return ":three:";
                case SeIconChar.BoxedNumber4:
                    return ":four:";
                case SeIconChar.BoxedNumber5:
                    return ":five:";
                case SeIconChar.BoxedNumber6:
                    return ":six:";
                case SeIconChar.BoxedNumber7:
                    return ":seven:";
                case SeIconChar.BoxedNumber8:
                    return ":eight:";
                case SeIconChar.BoxedNumber9:
                    return ":nine:";
                case SeIconChar.BoxedNumber10:
                    break;
                case SeIconChar.BoxedNumber11:
                    break;
                case SeIconChar.BoxedNumber12:
                    break;
                case SeIconChar.BoxedNumber13:
                    break;
                case SeIconChar.BoxedNumber14:
                    break;
                case SeIconChar.BoxedNumber15:
                    break;
                case SeIconChar.BoxedNumber16:
                    break;
                case SeIconChar.BoxedNumber17:
                    break;
                case SeIconChar.BoxedNumber18:
                    break;
                case SeIconChar.BoxedNumber19:
                    break;
                case SeIconChar.BoxedNumber20:
                    break;
                case SeIconChar.BoxedNumber21:
                    break;
                case SeIconChar.BoxedNumber22:
                    break;
                case SeIconChar.BoxedNumber23:
                    break;
                case SeIconChar.BoxedNumber24:
                    break;
                case SeIconChar.BoxedNumber25:
                    break;
                case SeIconChar.BoxedNumber26:
                    break;
                case SeIconChar.BoxedNumber27:
                    break;
                case SeIconChar.BoxedNumber28:
                    break;
                case SeIconChar.BoxedNumber29:
                    break;
                case SeIconChar.BoxedNumber30:
                    break;
                case SeIconChar.BoxedNumber31:
                    break;
                case SeIconChar.BoxedPlus:
                    break;
                case SeIconChar.BoxedQuestionMark:
                    return ":grey_question:";
                case SeIconChar.BoxedStar:
                    return ":stars:";
                case SeIconChar.BoxedRoman1:
                    return "Ⅰ";
                case SeIconChar.BoxedRoman2:
                    return "Ⅱ";
                case SeIconChar.BoxedRoman3:
                    return "Ⅲ";
                case SeIconChar.BoxedRoman4:
                    return "Ⅳ";
                case SeIconChar.BoxedRoman5:
                    return "Ⅴ";
                case SeIconChar.BoxedRoman6:
                    return "Ⅵ";
                case SeIconChar.BoxedLetterA:
                    return ":regional_indicator_a:";
                case SeIconChar.BoxedLetterB:
                    return ":regional_indicator_b:";
                case SeIconChar.BoxedLetterC:
                    return ":regional_indicator_c:";
                case SeIconChar.BoxedLetterD:
                    return ":regional_indicator_d:";
                case SeIconChar.BoxedLetterE:
                    return ":regional_indicator_e:";
                case SeIconChar.BoxedLetterF:
                    return ":regional_indicator_f:";
                case SeIconChar.BoxedLetterG:
                    return ":regional_indicator_g:";
                case SeIconChar.BoxedLetterH:
                    return ":regional_indicator_h:";
                case SeIconChar.BoxedLetterI:
                    return ":regional_indicator_i:";
                case SeIconChar.BoxedLetterJ:
                    return ":regional_indicator_j:";
                case SeIconChar.BoxedLetterK:
                    return ":regional_indicator_k:";
                case SeIconChar.BoxedLetterL:
                    return ":regional_indicator_l:";
                case SeIconChar.BoxedLetterM:
                    return ":regional_indicator_m:";
                case SeIconChar.BoxedLetterN:
                    return ":regional_indicator_n:";
                case SeIconChar.BoxedLetterO:
                    return ":regional_indicator_o:";
                case SeIconChar.BoxedLetterP:
                    return ":regional_indicator_p:";
                case SeIconChar.BoxedLetterQ:
                    return ":regional_indicator_q:";
                case SeIconChar.BoxedLetterR:
                    return ":regional_indicator_r:";
                case SeIconChar.BoxedLetterS:
                    return ":regional_indicator_s:";
                case SeIconChar.BoxedLetterT:
                    return ":regional_indicator_t:";
                case SeIconChar.BoxedLetterU:
                    return ":regional_indicator_u:";
                case SeIconChar.BoxedLetterV:
                    return ":regional_indicator_v:";
                case SeIconChar.BoxedLetterW:
                    return ":regional_indicator_w:";
                case SeIconChar.BoxedLetterX:
                    return ":regional_indicator_x:";
                case SeIconChar.BoxedLetterY:
                    return ":regional_indicator_y:";
                case SeIconChar.BoxedLetterZ:
                    return ":regional_indicator_z:";
                case SeIconChar.Circle:
                    return ":record_button:";
                case SeIconChar.Square:
                    return ":stop_button:";
                case SeIconChar.Cross:
                    return ":negative_squared_cross_mark:";
                case SeIconChar.Triangle:
                    return ":arrow_up_small:";
                case SeIconChar.Hexagon:
                    break;
                case SeIconChar.Prohibited:
                    return ":no_entry_sign:";
                case SeIconChar.Dice:
                    return ":game_die:";
                case SeIconChar.Debuff:
                    return ":arrow_down:";
                case SeIconChar.Buff:
                    return ":arrow_up:";
                case SeIconChar.CrossWorld:
                    return ":globe_with_meridians:";
                case SeIconChar.EurekaLevel:
                    break;
                case SeIconChar.LinkMarker:
                    return ":arrow_forward:";
                case SeIconChar.Glamoured:
                    break;
                case SeIconChar.GlamouredDyed:
                    return ":heavy_plus_sign:";
                case SeIconChar.QuestSync:
                    break;
                case SeIconChar.QuestRepeatable:
                    break;
                case SeIconChar.Instance1:
                    return ":one:";
                case SeIconChar.Instance2:
                    return ":two:";
                case SeIconChar.Instance3:
                    return ":three:";
                case SeIconChar.Instance4:
                    return ":four:";
                case SeIconChar.Instance5:
                    return ":five:";
                case SeIconChar.Instance6:
                    return ":six:";
                case SeIconChar.Instance7:
                    return ":seven:";
                case SeIconChar.Instance8:
                    return ":eight:";
                case SeIconChar.Instance9:
                    return ":nine:";
                case SeIconChar.InstanceMerged:
                    break;
                case SeIconChar.LocalTimeEn:
                case SeIconChar.LocalTimeDe:
                case SeIconChar.LocalTimeFr:
                case SeIconChar.LocalTimeJa:
                    return "LT";
                case SeIconChar.ServerTimeEn:
                case SeIconChar.ServerTimeDe:
                case SeIconChar.ServerTimeFr:
                case SeIconChar.ServerTimeJa:
                    return "ST";
                case SeIconChar.EorzeaTimeEn:
                case SeIconChar.EorzeaTimeDe:
                case SeIconChar.EorzeaTimeFr:
                case SeIconChar.EorzeaTimeJa:
                    return "ET";
            }

            return null;
        }
    }
}
