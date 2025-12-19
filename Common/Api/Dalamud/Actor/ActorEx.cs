using System;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;

namespace Dalamud.Divination.Common.Api.Dalamud.Actor;

public static class ActorEx
{
    public static async Task<IPlayerCharacter> GetLocalPlayerAsync(this IObjectTable objectTable, TimeSpan? delay = null, CancellationToken token = default)
    {
        delay ??= TimeSpan.FromMilliseconds(200);

        while (!token.IsCancellationRequested)
        {
            var player = objectTable.LocalPlayer;
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
