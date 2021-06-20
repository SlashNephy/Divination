using System;

namespace Dalamud.Divination.Common.Api.Ipc
{
    public interface IDalamudIpcClient : IDisposable
    {
        public void RegisterCallback(string target, Action<IpcMessage> callback);
        public void SendPing(string target);
        public void SendMessage(string target, string @event, dynamic message);
    }
}
