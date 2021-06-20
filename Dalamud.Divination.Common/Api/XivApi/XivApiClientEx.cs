namespace Dalamud.Divination.Common.Api.XivApi
{
    public static class XivApiClientEx
    {
        public static XivApiResponse Get(this IXivApiClient client, string content, uint id, bool ignoreCache = false)
        {
            return client.GetAsync(content, id, ignoreCache).GetAwaiter().GetResult();
        }

        public static XivApiResponse GetCharacter(this IXivApiClient client, string name, string world, bool ignoreCache = false)
        {
            return client.GetCharacterAsync(name, world, ignoreCache).GetAwaiter().GetResult();
        }
    }
}
