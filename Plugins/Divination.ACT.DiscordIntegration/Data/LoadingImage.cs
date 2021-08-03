using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Divination.ACT.DiscordIntegration.Data
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum LoadingImage
    {
        Eorzea = 1,
        Limsa = 2,
        Gridania = 3,
        Uldah = 4,
        Ishgard = 5,
        MorDhona = 6,
        Abalathia = 7,
        Dravania = 8,
        GyrAbania = 9,
        Othard = 10,
        Kugane = 11,
        Unknown = 12,
        Lakeland = 13,
        Kholusia = 14,
        AmhAraeng = 15,
        IlMheg = 16,
        Raktika = 17,
        Tempest = 18,
        Crystarium = 19,
        Eulmore = 20
    }

    internal static class LoadingImageEx
    {
        public static string? GetImageKey(this LoadingImage image)
        {
            var id = (byte) image;
            if (id < 1 || id > 20)
            {
                return null;
            }

            return $"li_{id}";
        }
    }
}
