using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.SseClient.Handlers;

public interface ISsePayloadEmitter : ISsePayloadHandler
{
    public bool CanEmit(XivChatType chatType);
    public void EmitChatMessage(XivChatType type, SeString sender, SeString message);
}