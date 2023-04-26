using Divination.Common;

#nullable enable
namespace Divination.ACT.DiscordIntegration
{
    internal static class XivApi
    {
        public static XivApiResponse GetClassJob(this XivApiClient client, uint id)
        {
            return client.Get("ClassJob", id);
        }

        public static XivApiResponse GetWorld(this XivApiClient client, uint id)
        {
            return client.Get("World", id);
        }

        public static XivApiResponse GetTerritoryType(this XivApiClient client, uint id)
        {
            return client.Get("TerritoryType", id);
        }

        public static bool IsContentTerritory(this XivApiClient client, uint id)
        {
            var value = client.GetTerritoryType(id);

            try
            {
                return value.Dynamic.GameContentLinks.ContentFinderCondition != null;
            }
            catch
            {
                return false;
            }
        }

        public static XivApiResponse? GetContentFinderCondition(this XivApiClient client, uint territoryId)
        {
            var value = client.GetTerritoryType(territoryId);

            try
            {
                uint id = value.Dynamic.GameContentLinks.ContentFinderCondition.TerritoryType[0];
                return client.Get("ContentFinderCondition", id);
            }
            catch
            {
                return null;
            }
        }

        public static XivApiResponse GetOnlineStatus(this XivApiClient client, uint id)
        {
            return client.Get("OnlineStatus", id);
        }
    }
}
