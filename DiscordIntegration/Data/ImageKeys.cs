using Lumina.Excel.GeneratedSheets;

namespace Divination.DiscordIntegration;

public static class ImageKeys
{
    public static string GetClassJobKey(ClassJob? job)
    {
        if (job == default)
        {
            return "job_0";
        }

        return $"job_{job.RowId}";
    }

    public static string GetLoadingImageKey(TerritoryType? territoryType)
    {
        var id = territoryType?.LoadingImage;
        if (id == default)
        {
            return "logo";
        }

        return $"li_{id}";
    }
}
