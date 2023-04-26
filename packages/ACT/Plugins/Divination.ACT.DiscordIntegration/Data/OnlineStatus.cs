using System.Diagnostics.CodeAnalysis;
using Divination.Common;

#nullable enable
namespace Divination.ACT.DiscordIntegration.Data
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum OnlineStatus : byte
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        GameQA = 1,
        GameMaster = 2,
        GameMaster2 = 3,
        EventParticipant = 4,
        Disconnected = 5,
        WaitingForFriendListApproval = 6,
        WaitingForLinkshellApproval = 7,
        WaitingForFreeCompanyApproval = 8,
        NotFound = 9,
        Offline = 10,
        BattleMentor = 11,
        Busy = 12,
        PvP = 13,
        PlayingTripleTriad = 14,
        ViewingCutScene = 15,
        UsingChocoboPorter = 16,
        AwayFromKeyboard = 17,
        CameraMode = 18,
        LookingForRepairs = 19,
        LookingToRepairs = 20,
        LookingToMeldMateria = 21,
        RolePlaying = 22,
        LookingForParty = 23,
        SwordForHire = 24,
        WaitingForDutyFinder = 25,
        RecruitingPartyMembers = 26,
        Mentor = 27,
        PvEMentor = 28,
        TradeMentor = 29,
        PvPMentor = 30,
        Returner = 31,
        NewAdventurer = 32,
        AllianceLeader = 33,
        AlliancePartyLeader = 34,
        AlliancePartyMember = 35,
        PartyLeader = 36,
        PartyMember = 37,
        CrossWorldPartyLeader = 38,
        CrossWorldPartyMember = 39,
        AnotherWorld = 40,
        SharingDuty = 41,
        SimilarDuty = 42,
        InDuty = 43,
        TrialAdventurer = 44,
        Online = 47
    }

    internal static class OnlineStatusEx
    {
        private static XivApiResponse GetXivApi(this OnlineStatus status)
        {
            return Plugin.XivApi.GetOnlineStatus((uint) status);
        }

        public static string? GetLocalizedName(this OnlineStatus status)
        {
            return GetXivApi(status).GetLocalizedString("Name");
        }

        public static Emoji? GetEmoji(this OnlineStatus status)
        {
            switch (status)
            {
                case OnlineStatus.GameQA:
                    break;
                case OnlineStatus.GameMaster:
                    break;
                case OnlineStatus.GameMaster2:
                    break;
                case OnlineStatus.EventParticipant:
                    break;
                case OnlineStatus.Disconnected:
                    return new Emoji("disconnected", "728859083968348190");
                case OnlineStatus.WaitingForFriendListApproval:
                    break;
                case OnlineStatus.WaitingForLinkshellApproval:
                    break;
                case OnlineStatus.WaitingForFreeCompanyApproval:
                    break;
                case OnlineStatus.NotFound:
                    break;
                case OnlineStatus.Offline:
                    return new Emoji("offline", "728859083591123076");
                case OnlineStatus.Busy:
                    return new Emoji("busy", "728859083997839432");
                case OnlineStatus.PlayingTripleTriad:
                    return new Emoji("triple_triad", "728859083825872937");
                case OnlineStatus.ViewingCutScene:
                    return new Emoji("cutscene", "728859083872010280");
                case OnlineStatus.UsingChocoboPorter:
                    return new Emoji("chocobo_porter", "728859083959959633");
                case OnlineStatus.AwayFromKeyboard:
                    return new Emoji("afk", "728859083834392678");
                case OnlineStatus.CameraMode:
                    return new Emoji("group_pose", "728859083586928662");
                case OnlineStatus.LookingForRepairs:
                    break;
                case OnlineStatus.LookingToRepairs:
                    break;
                case OnlineStatus.LookingToMeldMateria:
                    return new Emoji("looking_for_meld", "728859083876073522");
                case OnlineStatus.RolePlaying:
                    return new Emoji("rp", "728859083607638027");
                case OnlineStatus.LookingForParty:
                    return new Emoji("looking_for_party", "728859083603574816");
                case OnlineStatus.SwordForHire:
                    break;
                case OnlineStatus.WaitingForDutyFinder:
                    return new Emoji("duty_finder", "728859083616026665");
                case OnlineStatus.RecruitingPartyMembers:
                    return new Emoji("party_recruiting", "728859083805032569");
                case OnlineStatus.Mentor:
                    return new Emoji("mentor", "728859083842781214");
                case OnlineStatus.BattleMentor:
                case OnlineStatus.PvEMentor:
                    return new Emoji("battle_mentor", "728859083804770325");
                case OnlineStatus.TradeMentor:
                    return new Emoji("trade_mentor", "728859083519688755");
                case OnlineStatus.PvPMentor:
                    return new Emoji("pvp_mentor", "728859083611963473");
                case OnlineStatus.Returner:
                    return new Emoji("returner", "728859083851038750");
                case OnlineStatus.NewAdventurer:
                    return new Emoji("novice", "728859083679203329");
                case OnlineStatus.AllianceLeader:
                    break;
                case OnlineStatus.AlliancePartyLeader:
                    break;
                case OnlineStatus.AlliancePartyMember:
                    break;
                case OnlineStatus.PartyLeader:
                    return new Emoji("party_leader", "728859083788255312");
                case OnlineStatus.PartyMember:
                    return new Emoji("party_member", "728859083775410266");
                case OnlineStatus.CrossWorldPartyLeader:
                    return new Emoji("cwpt_leader", "728859083775541248");
                case OnlineStatus.CrossWorldPartyMember:
                    return new Emoji("cwpt_member", "728859083330814064");
                case OnlineStatus.AnotherWorld:
                case OnlineStatus.SharingDuty:
                case OnlineStatus.SimilarDuty:
                    return new Emoji("in_duty", "728859083783929886");
                case OnlineStatus.PvP:
                case OnlineStatus.InDuty:
                    return new Emoji("in_world_duty", "728859083796643890");
                case OnlineStatus.TrialAdventurer:
                    break;
                case OnlineStatus.Online:
                    return new Emoji("online", "728859083758764104");
            }

            return null;
        }

        public static bool ShouldOverrideJobEmojiOnField(this OnlineStatus status)
        {
            switch (status)
            {
                case OnlineStatus.Online:
                case OnlineStatus.Mentor:
                case OnlineStatus.BattleMentor:
                case OnlineStatus.PvEMentor:
                case OnlineStatus.TradeMentor:
                case OnlineStatus.PvPMentor:
                case OnlineStatus.Returner:
                case OnlineStatus.NewAdventurer:
                case OnlineStatus.AnotherWorld:
                case OnlineStatus.SharingDuty:
                case OnlineStatus.SimilarDuty:
                case OnlineStatus.PvP:
                case OnlineStatus.InDuty:
                    return false;
                default:
                    return true;
            }
        }

        public static bool ShouldOverrideJobEmojiOnInstance(this OnlineStatus status)
        {
            switch (status)
            {
                case OnlineStatus.PartyLeader:
                case OnlineStatus.PartyMember:
                case OnlineStatus.AlliancePartyLeader:
                case OnlineStatus.AlliancePartyMember:
                case OnlineStatus.CrossWorldPartyLeader:
                case OnlineStatus.CrossWorldPartyMember:
                    return false;
                default:
                    return status.ShouldOverrideJobEmojiOnField();
            }
        }
    }
}
