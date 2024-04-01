namespace Divination.DiscordIntegration.Data;

internal enum OnlineStatus : byte
{
    InDuty = 43,
    Online = 47,
}

internal static class OnlineStatusEx
{
    public static string? GetLocalizedName(this OnlineStatus status)
    {
        var data = DiscordIntegration.Instance.Dalamud.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.OnlineStatus>()!.GetRow((uint)status);

        return data?.Name?.RawString;
    }
}
