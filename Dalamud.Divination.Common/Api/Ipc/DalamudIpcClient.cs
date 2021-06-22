using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Ipc
{
    [Obsolete("Will be removed after Dalamud API version 3.")]
    internal sealed class DalamudIpcClient : IDalamudIpcClient
    {
        private readonly string name;
        private readonly DalamudPluginInterface @interface;

        private readonly List<(string target, Action<IpcMessage> callback)> subscriptions = new();
        private readonly object subscriptionsLock = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(DalamudIpcClient));

        public DalamudIpcClient(string name, DalamudPluginInterface @interface)
        {
            this.name = name;
            this.@interface = @interface;

            @interface.SubscribeAny(OnIpcMessage);
            logger.Debug("IPC to {Name} was subscribed", name);
        }

        public void RegisterCallback(string target, Action<IpcMessage> callback)
        {
            lock (subscriptionsLock)
            {
                subscriptions.Add((target, callback));
            }
        }

        public void SendPing(string target)
        {
            dynamic obj = new ExpandoObject();
            obj.Message = new IpcMessage(target, "ping", null);

            @interface.SendMessage(obj);
        }

        public void SendMessage(string target, string @event, dynamic message)
        {
            dynamic obj = new ExpandoObject();
            obj.Message = new IpcMessage(target, @event, message);

            @interface.SendMessage(obj);
        }

        private void OnIpcMessage(string target, ExpandoObject obj)
        {
            if (!((IDictionary<string, object>) obj).ContainsKey("Message"))
            {
                return;
            }

            if (target != name)
            {
                return;
            }

            if (((dynamic) obj).Message is not IpcMessage message)
            {
                return;
            }

            foreach (var (_, action) in subscriptions.Where(x => x.target == target))
            {
                try
                {
                    switch (message.Event)
                    {
                        case "ping":
                            logger.Verbose("Ping received from {Target}", target);
                            continue;
                        default:
                            action(message);
                            continue;
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "Error occurred while OnIpcMessage");
                }
            }
        }

        public void Dispose()
        {
            @interface.UnsubscribeAny();
            logger.Verbose("IPC to {Name} was unsubscribed", name);

            logger.Dispose();
        }
    }
}
