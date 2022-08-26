using System;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Voiceroid2Proxy
{
    public interface IVoiceroid2ProxyClient : IDisposable
    {
        public Task TalkAsync(string text);
    }
}
