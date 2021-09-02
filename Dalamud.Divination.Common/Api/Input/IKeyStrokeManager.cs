using System;

namespace Dalamud.Divination.Common.Api.Input
{
    public interface IKeyStrokeManager : IDisposable
    {
        public void Send(string rawKeys);
    }
}
