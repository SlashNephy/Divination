using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game.Network;
using Dalamud.Logging;

namespace Dalamud.Divination.Common.Api.Network
{
    public sealed class NetworkInterceptor : INetworkInterceptor
    {
        private readonly List<INetworkHandler> handlers = new();
        private readonly NetworkDataParser parser;

        public NetworkInterceptor(GameNetwork gameNetwork)
        {
            parser = new NetworkDataParser(gameNetwork);
            parser.OnNetworkContext += Consume;
        }

        public void AddHandler(INetworkHandler handler)
        {
            handlers.Add(handler);
        }

        public void RemoveHandler(INetworkHandler handler)
        {
            handlers.Remove(handler);
        }

        private void Consume(NetworkContext context)
        {
            foreach (var handler in handlers)
            {
                Task.Run(() =>
                {
                    ConsumeEach(handler, context);
                });
            }
        }

        private static void ConsumeEach(INetworkHandler handler, NetworkContext context)
        {
            try
            {
                switch (context.Direction)
                {
                    case NetworkMessageDirection.ZoneDown when handler.CanHandleReceivedMessage(context):
                        handler.HandleReceivedMessage(context);
                        return;
                    case NetworkMessageDirection.ZoneUp when handler.CanHandleSentMessage(context):
                        handler.HandleSentMessage(context);
                        return;
                }
            }
            catch (Exception exception)
            {
                PluginLog.Error(exception, "Error occurred while ConsumeEach");
            }
        }

        public void Dispose()
        {
            parser.OnNetworkContext -= Consume;
            parser.Dispose();

            // ReSharper disable once SuspiciousTypeConversion.Global
            foreach (var handler in handlers.OfType<IDisposable>())
            {
                handler.Dispose();
            }
            handlers.Clear();
        }
    }
}
