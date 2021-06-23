using System.Threading;
using System.Threading.Tasks;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Actors.Types;

namespace Dalamud.Divination.Common.Api.Dalamud.Actor
{
    public static class ActorEx
    {
        public static async Task<PlayerCharacter> GetPlayerAsync(this ClientState state, CancellationToken token)
        {
            while (true)
            {
                var player = state.LocalPlayer;
                if (player != null)
                {
                    return player;
                }

                await Task.Delay(200, token);
            }
        }
    }
}
