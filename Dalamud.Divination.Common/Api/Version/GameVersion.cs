using System.IO;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Version
{
    public static class GameVersion
    {
        // "C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game\ffxivgame.ver"
        private static string VersionPath => Path.Combine(DivinationEnvironment.GameDirectory, "ffxivgame.ver");

        public static async Task<string> ReadCurrentPlainAsync()
        {
            var content = await File.ReadAllTextAsync(VersionPath);
            return content.Trim();
        }

        public static string ReadCurrentPlain()
        {
            var content = File.ReadAllText(VersionPath);
            return content.Trim();
        }

        public static async Task<Game.GameVersion> ReadCurrentAsync()
        {
            return await ReadCurrentPlainAsync();
        }

        public static Game.GameVersion ReadCurrent()
        {
            return ReadCurrentPlain();
        }
    }
}
