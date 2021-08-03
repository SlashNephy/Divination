using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common.Models;

#nullable enable
namespace Divination.ACT
{
    public abstract partial class DivinationActPlugin<TW, TU, TS>
    {
        public static Player? CurrentPlayer => DataRepository.GetPlayer();

        public static Combatant? CurrentActor
        {
            get
            {
                var playerId = DataRepository.GetCurrentPlayerID();

                return DataRepository.GetCombatantList()
                    .FirstOrDefault(x => x.ID == playerId);
            }
        }

        public static void Speak(string text)
        {
            ActGlobals.oFormActMain?.TTS(text);
        }

        public static async Task PostDiscordWebhook(string url, string content, string? username = null,
            string? avatarUrl = null)
        {
            var parameters = new Dictionary<string, string?>
            {
                {"content", content},
                {"username", username},
                {"avatar_url", avatarUrl}
            };
            var payload = new FormUrlEncodedContent(parameters);

            await HttpClient.PostAsync(url, payload);
        }
    }
}
