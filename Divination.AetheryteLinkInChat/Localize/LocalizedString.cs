using Dalamud;

namespace Divination.AetheryteLinkInChat.Localize;

public sealed class LocalizedString
{
    private string? cached;

    public string En { get; init; } = string.Empty;
    public string? Ja { get; init; }

    public static explicit operator string(LocalizedString localizedString)
    {
        return localizedString.ToString();
    }

    public override string ToString()
    {
        if (cached != default)
        {
            return cached;
        }

        switch (AetheryteLinkInChatPlugin.Instance.Dalamud.ClientState.ClientLanguage)
        {
            case ClientLanguage.Japanese when !string.IsNullOrWhiteSpace(Ja):
                cached = Ja;
                return Ja;
            default:
                cached = En;
                return En;
        }
    }

    public string Format(object? arg0)
    {
        return string.Format(ToString(), arg0);
    }

    public string Format(object? arg0, object? arg1)
    {
        return string.Format(ToString(), arg0, arg1);
    }

    public string Format(object? arg0, object? arg1, object? arg2)
    {
        return string.Format(ToString(), arg0, arg1, arg2);
    }

    public string Format(params object?[] args)
    {
        return string.Format(ToString(), args);
    }
}
