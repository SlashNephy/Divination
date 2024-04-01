namespace Divination.DiscordIntegration.Data;

internal enum OnlineStatus : byte
{
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
    Online = 47,
}

internal static class OnlineStatusEx
{
    public static string? GetLocalizedName(this OnlineStatus status)
    {
        var data = DiscordIntegration.Instance.Dalamud.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.OnlineStatus>()!.GetRow((uint)status);

        return data?.Name?.RawString;
    }
}
