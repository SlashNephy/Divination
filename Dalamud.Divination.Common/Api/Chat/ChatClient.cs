using System;
using System.Collections.Concurrent;
using Dalamud.Game.Internal.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Chat
{
    public class ChatClient : IChatClient
    {
        public static bool ShowHeader = true;
        public static SeString? Header;
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

            gui.OnChatMessage += OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (type == XivChatType.Notice)
            {
                CompleteChatQueue();
            }
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
            var message = FormatString(seString, false);

            EnqueueChat(new XivChatEntry
            {
                Type = type ?? NormalMessageType,
                Name = sender ?? string.Empty,
                MessageBytes = message.Encode()
            });
        }

        public void PrintError(SeString seString, string? sender = null, XivChatType? type = null)
        {
            var message = FormatString(seString, true);

            EnqueueChat(new XivChatEntry
            {
                Type = type ?? ErrorMessageType,
                Name = sender ?? string.Empty,
                MessageBytes = message.Encode()
            });
        }

        private SeString FormatString(SeString seString, bool error)
        {
            var message = ShowHeader ? (Header ?? DefaultHeader) : EmptySeString;
            var color = error ? ErrorMessageColor : NormalMessageColor;
            if (color > 0)
            {
                message = message.Append(new UIForegroundPayload(null, color));
            }

            message = message.Append(seString);

            return color > 0 ? message.Append(UIForegroundPayload.UIForegroundOff) : message;
        }

        private static SeString EmptySeString => new(Array.Empty<Payload>());

        private SeString DefaultHeader => new(new Payload[]
        {
            new UIForegroundPayload(null, HeaderColor),
            new TextPayload($"[{title}]"),
            UIForegroundPayload.UIForegroundOff,
            new TextPayload(" ")
        });

        private void CompleteChatQueue()
        {
            queue.CompleteAdding();

            if (!queue.IsCompleted)
            {
                foreach (var entry in queue.GetConsumingEnumerable())
                {
                    EnqueueChat(entry);
                }
            }
        }

        public void Dispose()
        {
            gui.OnChatMessage -= OnChatMessage;

            queue.Dispose();
        }
    }
}
