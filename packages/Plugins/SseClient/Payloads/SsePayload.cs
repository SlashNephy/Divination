using System;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

#pragma warning disable 8618

namespace Divination.SseClient.Payloads
{
    public class SsePayload
    {
        // TODO
        public string Sender { get; set; }
        private string? SenderRaw { get; set; }
        [JsonIgnore] private byte[] SenderBytes => Convert.FromBase64String(SenderRaw ?? Sender);
        [JsonIgnore] public SeString SenderSeString
        {
            get => SeString.Parse(SenderBytes);
            set
            {
                Sender = value.TextValue;
                SenderRaw = Convert.ToBase64String(value.Encode());
            }
        }

        public string Message { get; set; }
        private string? MessageRaw { get; set; }
        [JsonIgnore] private byte[] MessageBytes => Convert.FromBase64String(MessageRaw ?? Message);
        [JsonIgnore] public SeString MessageSeString
        {
            get => SeString.Parse(MessageBytes);
            set
            {
                Message = value.TextValue;
                MessageRaw = Convert.ToBase64String(value.Encode());
            }
        }

        [JsonProperty("type")] public ushort? ChatTypeId;
        [JsonIgnore] public XivChatType? ChatType
        {
            get => ChatTypeId == null ? null : (XivChatType) ChatTypeId;
            set
            {
                if (value != null)
                {
                    ChatTypeId = (ushort) value;
                }
            }
        }

        [JsonProperty("territory")] public uint? TerritoryTypeId;
        [JsonIgnore] public TerritoryType? TerritoryType
        {
            get => TerritoryTypeId == null ? null : SseClientPlugin.Instance.Dalamud.DataManager
                .GetExcelSheet<TerritoryType>()?
                .GetRow(TerritoryTypeId.Value);
            set => TerritoryTypeId = value?.RowId;
        }

        [JsonProperty("world")] public uint? WorldId;
        [JsonIgnore] public World? World
        {
            get => WorldId == null ? null : SseClientPlugin.Instance.Dalamud.DataManager
                .GetExcelSheet<World>()?
                .GetRow(WorldId.Value);
            set => WorldId = value?.RowId;
        }

        public string? Origin { get; set; }
        public Guid? OriginId { get; set; }
    }
}
