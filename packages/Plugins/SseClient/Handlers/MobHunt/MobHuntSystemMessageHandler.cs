using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;
using Lumina.Excel.GeneratedSheets;

namespace Divination.SseClient.Handlers.MobHunt
{
    public class MobHuntSystemMessageHandler : ISsePayloadReceiver, ISsePayloadEmitter
    {
        private const string EventId = "mobhunt_system";
        private const XivChatType MobHuntRankSsPopSystemMessageType = (XivChatType) 2105;
        private const string MobHuntRankSsPopSystemMessage = "特殊なリスキーモブの配下が、偵察活動を開始したようだ……";

        public bool CanEmit(XivChatType chatType) => SseClientPlugin.Instance.Config.SendMobHuntSystemMessages;

        public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
        {
            if (!message.TextValue.StartsWith(MobHuntRankSsPopSystemMessage))
            {
                return;
            }

            SseUtils.SendPayload(EventId, new SsePayload
            {
                ChatType = type,
                MessageSeString = message
            });
        }

        public bool CanReceive(string eventId)
        {
            return eventId == EventId && SseClientPlugin.Instance.Config.ReceiveMobHuntSystemMessages;
        }

        public void Receive(string eventId, SsePayload payload)
        {
            SseUtils.PrintSseChat(new XivChatEntry
            {
                Type = SseClientPlugin.Instance.Config.MobHuntSystemMessagesType,
                Name = $"{payload.TerritoryType?.PlaceName?.Value?.Name.RawString}{CrossWorldIconString}{payload.World?.Name?.RawString}",
                Message = payload.MessageSeString
            });
        }
    }
}
