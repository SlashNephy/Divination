using Dalamud.Divination.Common.Api.Dalamud;

namespace Dalamud.Divination.Common.Api.Localize;

public sealed class LocalizedString
{
    private string? cached;

    public string En { get; init; } = string.Empty;
    public string? Ja { get; init; }
    public string? Ge { get; init; }
    public string? Fr { get; init; }

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

        var api = ServiceContainer.Get<IDalamudApi>();
        switch (api?.ClientState.ClientLanguage)
        {
            case ClientLanguage.Japanese when !string.IsNullOrWhiteSpace(Ja):
                cached = Ja;
                return Ja;
            case ClientLanguage.German when !string.IsNullOrWhiteSpace(Ge):
                cached = Ge;
                return Ge;
            case ClientLanguage.French when !string.IsNullOrWhiteSpace(Fr):
                cached = Fr;
                return Fr;
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
