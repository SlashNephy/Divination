﻿using System;
using System.Collections.Concurrent;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Chat;

internal sealed class ChatClient : IChatClient
{
    public static bool ShowHeader = true;
    public static SeString? Header = null;
    public static ushort HeaderColor = 52;
    public static ushort NormalMessageColor = 0;
    public static ushort ErrorMessageColor = 14;
    public static XivChatType NormalMessageType = XivChatType.Echo;
    public static XivChatType ErrorMessageType = XivChatType.ErrorMessage;
    private readonly ChatGui gui;

    private readonly BlockingCollection<XivChatEntry> queue = new();

    private readonly string title;

    public ChatClient(string title, ChatGui gui, ClientState clientState)
    {
        this.title = title.Replace("Divination.", string.Empty);
        this.gui = gui;

        gui.ChatMessage += OnChatMessage;
        if (clientState.IsLoggedIn)
        {
            CompleteChatQueue();
        }
    }

    private static SeString EmptySeString => new(Array.Empty<Payload>());

    private SeString DefaultHeader => new(new UIForegroundPayload(HeaderColor),
        new TextPayload($"[{title}]"),
        UIForegroundPayload.UIForegroundOff,
        new TextPayload(" "));

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
            Message = FormatString(seString, false),
        });
    }

    public void PrintError(SeString seString, string? sender = null, XivChatType? type = null)
    {
        EnqueueChat(new XivChatEntry
        {
            Type = type ?? ErrorMessageType,
            Name = sender ?? string.Empty,
            Message = FormatString(seString, true),
        });
    }

    public void Dispose()
    {
        gui.ChatMessage -= OnChatMessage;

        queue.Dispose();
    }

    private void OnChatMessage(XivChatType type,
        uint senderId,
        ref SeString sender,
        ref SeString message,
        ref bool isHandled)
    {
        if (type == XivChatType.Notice)
        {
            CompleteChatQueue();
        }
    }

    private SeString FormatString(SeString seString, bool error)
    {
        var message = ShowHeader ? Header ?? DefaultHeader : EmptySeString;
        var color = error ? ErrorMessageColor : NormalMessageColor;
        if (color > 0)
        {
            message = message.Append(new UIForegroundPayload(color));
        }

        message = message.Append(seString);

        return color > 0 ? message.Append(UIForegroundPayload.UIForegroundOff) : message;
    }

    private void CompleteChatQueue()
    {
        queue.CompleteAdding();

        if (!queue.IsCompleted)
        {
            foreach (var entry in queue.GetConsumingEnumerable())
            {
                gui.PrintChat(entry);
            }
        }
    }
}
