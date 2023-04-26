using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Chat;

public static class ChatClientEx
{
    public static void Print(this IChatClient client,
        List<Payload> payloads,
        string? sender = null,
        XivChatType? type = null)
    {
        client.Print(new SeString(payloads), sender, type);
    }

    public static void Print(this IChatClient client,
        Action<List<Payload>> composer,
        string? sender = null,
        XivChatType? type = null)
    {
        var payloads = new List<Payload>();
        composer(payloads);
        client.Print(new SeString(payloads), sender, type);
    }

    public static void Print(this IChatClient client,
        string? sender = null,
        XivChatType? type = null,
        params Payload[] payloads)
    {
        client.Print(new SeString(payloads), sender, type);
    }

    public static void Print(this IChatClient client,
        string? sender = null,
        XivChatType? type = null,
        params object?[] contents)
    {
        var payload = new TextPayload(string.Join("\n", contents.Select(x => x?.ToString() ?? "null")));
        client.Print(new SeString(payload), sender, type);
    }

    public static void PrintError(this IChatClient client,
        List<Payload> payloads,
        string? sender = null,
        XivChatType? type = null)
    {
        client.PrintError(new SeString(payloads), sender, type);
    }

    public static void PrintError(this IChatClient client,
        Action<List<Payload>> composer,
        string? sender = null,
        XivChatType? type = null)
    {
        var payloads = new List<Payload>();
        composer(payloads);
        client.PrintError(new SeString(payloads), sender, type);
    }

    public static void PrintError(this IChatClient client,
        string? sender = null,
        XivChatType? type = null,
        params Payload[] payloads)
    {
        client.PrintError(new SeString(payloads), sender, type);
    }

    public static void PrintError(this IChatClient client,
        string? sender = null,
        XivChatType? type = null,
        params object?[] contents)
    {
        var payload = new TextPayload(string.Join("\n", contents.Select(x => x?.ToString() ?? "null")));
        client.PrintError(new SeString(payload), sender, type);
    }
}
