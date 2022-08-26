using System;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects.SubKinds;

namespace Dalamud.Divination.Common.Api.Dalamud.Actor
{
    public static class ActorEx
    {
        public static async Task<PlayerCharacter> GetLocalPlayerAsync(this ClientState state,
            TimeSpan? delay = null,
            CancellationToken token = default)
        {
            delay ??= TimeSpan.FromMilliseconds(200);

            while (!token.IsCancellationRequested)
            {
                var player = state.LocalPlayer;
                if (player != null)
                {
                    return player;
                }

                await Task.Delay(delay.Value, token);
            }

            token.ThrowIfCancellationRequested();
            throw new OperationCanceledException();
        }
    }
}
