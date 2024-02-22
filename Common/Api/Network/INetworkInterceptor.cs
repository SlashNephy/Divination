using System;

namespace Dalamud.Divination.Common.Api.Network;

public interface INetworkInterceptor : IDisposable
{
    public IOpcodeDetectorManager OpcodeDetectorManager { get; }
    public void AddHandler(INetworkHandler handler);
    public void RemoveHandler(INetworkHandler handler);
}
