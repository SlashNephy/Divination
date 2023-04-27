using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.Internal
{
    public class SsePayloadHandlerManager : IDisposable
    {
        private readonly List<ISsePayloadHandler> handlers = new()
        {
            // TODO
        };

        private readonly SseConnectionManager connectionManager;

        public SsePayloadHandlerManager(SseConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;

            SseClientPlugin.Instance.Dalamud.ChatGui.ChatMessage += OnChatMessage;
            connectionManager.SsePayload += OnSsePayload;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            foreach (var emitter in handlers.OfType<ISsePayloadEmitter>().Where(x => x.CanEmit(type)))
            {
                emitter.EmitChatMessage(type, sender, message);
            }
        }

        private void OnSsePayload(string eventId, SsePayload payload)
        {
            foreach (var receiver in handlers.OfType<ISsePayloadReceiver>().Where(x => x.CanReceive(eventId)))
            {
                receiver.Receive(eventId, payload);
            }
        }

        public void Dispose()
        {
            SseClientPlugin.Instance.Dalamud.ChatGui.ChatMessage -= OnChatMessage;
            connectionManager.SsePayload -= OnSsePayload;

            foreach (var handler in handlers.OfType<IDisposable>())
            {
                handler.Dispose();
            }
        }
    }
}
