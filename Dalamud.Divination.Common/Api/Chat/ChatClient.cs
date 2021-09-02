using System;
using System.Collections.Concurrent;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Chat
{
    internal sealed class ChatClient : IChatClient
    {
        public static bool ShowHeader = true;
        public static SeString? Header = null;
        public static ushort HeaderColor = 52;
        public static ushort NormalMessageColor = 0;
        public static ushort ErrorMessageColor = 14;
        public static XivChatType NormalMessageType = XivChatType.Echo;
        public static XivChatType ErrorMessageType = XivChatType.ErrorMessage;

        private readonly string title;
        private readonly ChatGui gui;

        private readonly BlockingCollection<XivChatEntry> queue = new();

        public ChatClient(string title, ChatGui gui)
        {
            this.title = title;
            this.gui = gui;
        }

        public void EnqueueChat(XivChatEntry entry)
        {
            if (!queue.IsAddingCompleted)
            {
                queue.Add(entry);
            }
            else
            {
                gui.PrintChat(entry);
            }
        }

        public void Print(SeString seString, string? sender = null, XivChatType? type = null)
        {
            EnqueueChat(new XivChatEntry
            {
                Type = type ?? NormalMessageType,
                Name = sender ?? string.Empty,
                Message = FormatString(seString, false)
            });
        }

        public void PrintError(SeString seString, string? sender = null, XivChatType? type = null)
        {
            EnqueueChat(new XivChatEntry
            {
                Type = type ?? ErrorMessageType,
                Name = sender ?? string.Empty,
                Message = FormatString(seString, true)
            });
        }

        private SeString FormatString(SeString seString, bool error)
        {
            var message = ShowHeader ? (Header ?? DefaultHeader) : EmptySeString;
            var color = error ? ErrorMessageColor : NormalMessageColor;
            if (color > 0)
            {
                message = message.Append(new UIForegroundPayload(color));
            }

            message = message.Append(seString);

            return color > 0 ? message.Append(UIForegroundPayload.UIForegroundOff) : message;
        }

        private static SeString EmptySeString => new(Array.Empty<Payload>());

        private SeString DefaultHeader => new(
            new UIForegroundPayload(HeaderColor),
            new TextPayload($"[{title}]"),
            UIForegroundPayload.UIForegroundOff,
            new TextPayload(" "));

        public void Dispose()
        {
            queue.Dispose();
        }
    }
}
