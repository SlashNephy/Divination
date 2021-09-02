using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Input;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Divination.Common.Api.Voiceroid2Proxy;
using Dalamud.Divination.Common.Api.XivApi;

namespace Dalamud.Divination.Common.Api
{
    public interface IDivinationApi
    {
        public IChatClient Chat { get; }

        public ICommandProcessor? Command { get; }

        public IBugReporter Reporter { get; }

        public ITextureManager Texture { get; }

        public IVersionManager Version { get; }

        public IVoiceroid2ProxyClient Voiceroid2Proxy { get; }

        public IXivApiClient XivApi { get; }

        public IKeyStrokeManager KeyStroke { get; }

        public INetworkInterceptor Network { get; }
    }
}
