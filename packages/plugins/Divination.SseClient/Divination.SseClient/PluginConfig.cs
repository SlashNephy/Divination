using System;
using System.Text.Json.Serialization;
using Dalamud.Configuration;
using Dalamud.Game.Text;

namespace Divination.SseClient
{
    public class PluginConfig : IPluginConfiguration
    {
        public string EndpointUrl = string.Empty;
        public string Token = string.Empty;

        public bool EnableDoNotDisturb = false;
        public bool ReceiveDiscordMessages = true;
        public bool ReceiveMobHuntLsMessages = true;
        public bool ReceiveMobHuntCwlsMessages = true;
        public bool ReceiveMobHuntDiscordMessages = true;
        public bool ReceiveMobHuntShoutMessages = true;
        public bool ReceiveMobHuntYellMessages = true;
        public bool ReceiveMobHuntSayMessages = false;
        public bool ReceiveMobHuntActor = false;
        public bool ShowMobHuntActorChat = false;
        public bool ShowMobHuntActorWidget = false;
        public bool ReceiveMobHuntSystemMessages = true;
        public bool ReceiveFaloopSpawnMessages = true;
        public bool ReceiveFaloopKillMessages = true;
        public bool ReceiveFaloopSightMessages = false;
        public bool ReceiveFaloopSystemMessages = false;
        public bool ReceiveGateAnnouncements = true;
        public bool ShowIconOnReceivedMessages = false;

        public bool SendFcMessages = true;
        public bool SendLs1Messages = false;
        public bool SendCwls1Messages = false;
        public bool SendShoutMessages = true;
        public bool SendYellMessages = true;
        public bool SendSayMessages = true;
        public bool SendMobHuntActor = true;
        public bool SendMobHuntSystemMessages = true;
        public bool SendGateAnnouncements = true;

        [JsonIgnore]
        private static readonly Array Types = Enum.GetValues(typeof(XivChatType));

        private static int GetTypeIndex(object value)
        {
            return Array.IndexOf(Types, value);
        }

        public int FCMessagesTypeIndex = GetTypeIndex(XivChatType.FreeCompany);
        public int DiscordMessagesTypeIndex = GetTypeIndex(XivChatType.FreeCompany);
        public int MobHuntLsMessagesTypeIndex = GetTypeIndex(XivChatType.Ls1);
        public int MobHuntCwlsMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntDiscordMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntShoutMessagesTypeIndex = GetTypeIndex(XivChatType.Shout);
        public int MobHuntYellMessagesTypeIndex = GetTypeIndex(XivChatType.Yell);
        public int MobHuntSayMessagesTypeIndex = GetTypeIndex(XivChatType.Say);
        public int MobHuntSystemMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntFaloopSpawnMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntFaloopKillMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntFaloopSightMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);
        public int MobHuntFaloopSystemMessagesTypeIndex = GetTypeIndex(XivChatType.Echo);
        public int LsMessagesTypeIndex = GetTypeIndex(XivChatType.Ls1);
        public int CwlsMessagesTypeIndex = GetTypeIndex(XivChatType.CrossLinkShell1);

        private static XivChatType GetTypeByIndex(int index)
        {
            return (XivChatType) Types.GetValue(index)!;
        }

        [JsonIgnore]
        public XivChatType DiscordMessagesType => GetTypeByIndex(DiscordMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntLsMessagesType => GetTypeByIndex(MobHuntLsMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntCwlsMessagesType => GetTypeByIndex(MobHuntCwlsMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntDiscordMessagesType => GetTypeByIndex(MobHuntDiscordMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntShoutMessagesType => GetTypeByIndex(MobHuntShoutMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntYellMessagesType => GetTypeByIndex(MobHuntYellMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntSayMessagesType => GetTypeByIndex(MobHuntSayMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntSystemMessagesType => GetTypeByIndex(MobHuntSystemMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntFaloopSpawnMessagesType => GetTypeByIndex(MobHuntFaloopSpawnMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntFaloopKillMessagesType => GetTypeByIndex(MobHuntFaloopKillMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntFaloopSightMessagesType => GetTypeByIndex(MobHuntFaloopSightMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType MobHuntFaloopSystemMessagesType => GetTypeByIndex(MobHuntFaloopSystemMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType LsMessagesType => GetTypeByIndex(LsMessagesTypeIndex);
        [JsonIgnore]
        public XivChatType CwlsMessagesType => GetTypeByIndex(CwlsMessagesTypeIndex);

        public bool FilterMobHuntLsMessages = true;
        public bool FilterMobHuntShoutMessages = true;
        public bool FilterSameMessageIn5Min = true;
        public bool FilterNoticeMessages = false;
        public bool FilterNoviceNetworkMessages = false;
        public bool FilterDebugMessages = false;
        public bool MoveDebugMessagesToEcho = false;
        public bool FilterSonarRankBMessages = false;
        public bool FilterEchoDuplication = false;
        public bool FilterOldMobKills = true;
        public bool FilterSameMapLinks = false;

        public int Version { get; set; } = 1;
    }
}
