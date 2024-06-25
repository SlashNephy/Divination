using Dalamud.Divination.Common.Api.Localize;

namespace Divination.FaloopIntegration;

public static class Localization
{
    public static readonly LocalizedString Connected = new()
    {
        En = "Connected to Faloop!",
        Ja = "Faloop に接続しました！",
    };

    public static readonly LocalizedString Disconnected = new()
    {
        En = "Disconnected from Faloop...",
        Ja = "Faloop から切断されました...",
    };

    public static readonly LocalizedString HasSpawned = new()
    {
        En = "has spawned!",
        Ja = "が湧きました！",
    };

    public static readonly LocalizedString WasKilled = new()
    {
        En = "was killed.",
        Ja = "が討伐されました。",
    };

    public static readonly LocalizedString TimespanDaysAgo = new()
    {
        En = "{0} days ago",
        Ja = "{0}日前",
    };

    public static readonly LocalizedString TimespanHoursAgo = new()
    {
        En = "{0} hours ago",
        Ja = "{0}時間前",
    };

    public static readonly LocalizedString TimespanMinutesAgo = new()
    {
        En = "{0} minutes ago",
        Ja = "{0}分前",
    };

    public static readonly LocalizedString TimespanSecondsAgo = new()
    {
        En = "{0} seconds ago",
        Ja = "{0}秒前",
    };

    public static readonly LocalizedString ConfigWindowTitle = new()
    {
        En = "{0} Configuration",
        Ja = "{0} 設定",
    };

    public static readonly LocalizedString SaveConfigButton = new()
    {
        En = "Save & Close",
        Ja = "保存して閉じる",
    };

    public static readonly LocalizedString Account = new()
    {
        En = "Faloop Account",
        Ja = "Faloop アカウント",
    };

    public static readonly LocalizedString AccountUsername = new()
    {
        En = "Username",
        Ja = "ユーザー名",
    };

    public static readonly LocalizedString AccountPassword = new()
    {
        En = "Password",
        Ja = "パスワード",
    };

    public static readonly LocalizedString AccountNotSet = new()
    {
        En = "Faloop account not set up. Type /faloop to log in with your Faloop account.",
        Ja = "Faloop アカウントが設定されていません。/faloop コマンドを実行して、Faloop アカウントでログインしてください。",
    };

    public static readonly LocalizedString GeneralTab = new()
    {
        En = "General",
        Ja = "一般",
    };

    public static readonly LocalizedString RankSTab = new()
    {
        En = "Rank S",
        Ja = "Sモブ",
    };

    public static readonly LocalizedString FateTab = new()
    {
        En = "Special Fate",
        Ja = "特殊 F.A.T.E.",
    };

    public static readonly LocalizedString ReportChannel = new()
    {
        En = "Channel",
        Ja = "通知先",
    };

    public static readonly LocalizedString EnableSpawnReport = new()
    {
        En = "Enable Mob Spawn Report",
        Ja = "モブが湧いたときに通知",
    };

    public static readonly LocalizedString EnableDeathReport = new()
    {
        En = "Enable Mob Death Report",
        Ja = "モブが討伐されたときに通知",
    };

    public static readonly LocalizedString ReportJurisdiction = new()
    {
        En = "Jurisdiction",
        Ja = "モブの管轄",
    };

    public static readonly LocalizedString ReportJurisdictionDescription = new()
    {
        En = "For example, if you specify \"World,\" you will receive only notifications about your current world.",
        Ja = "例えば「World」を指定すると、現在地のワールドに関する通知のみを受け取ります。",
    };

    public static readonly LocalizedString IgnoreReports = new()
    {
        En = "Ignore Reports in the Following Conditions...",
        Ja = "以下の状態では通知を無効にする",
    };

    public static readonly LocalizedString ReportIgnoreInDuty = new()
    {
        En = "In Duty",
        Ja = "コンテンツ参加中",
    };

    public static readonly LocalizedString ReportIgnoreOrphanDeathReport = new()
    {
        En = "Orphan Death Report",
        Ja = "孤立した討伐報告",
    };

    public static readonly LocalizedString ReportIgnorePendingReport = new()
    {
        En = "Pending Report (e.g. coordinates not yet confirmed)",
        Ja = "座標が未確定の報告",
    };

    public static readonly LocalizedString ReportIgnoreOrphanDeathReportDescription = new()
    {
        En = "If enabled, the plugin will ignore reports of mob kills for which it has not received a spawn notification.",
        Ja = "有効にすると、湧き通知を受け取っていないモブの討伐報告を無視します。",
    };

    public static readonly LocalizedString ActiveMob = new()
    {
        En = "Faloop: Active Mobs",
        Ja = "Faloop: 現在のモブ情報",
    };

    public static readonly LocalizedString EnableActiveMobUi = new()
    {
        En = "Enable Active Mobs UI",
        Ja = "「現在のモブ情報」パネルを表示",
    };

    public static readonly LocalizedString HideActiveMobUiInDuty = new()
    {
        En = "Hide Active Mobs UI in Duty",
        Ja = "コンテンツ中は「現在のモブ情報」パネルを隠す",
    };

    public static readonly LocalizedString EnableSimpleReports = new()
    {
        En = "Enable simplified, condensed reports in chat",
        Ja = "簡素な通知メッセージを使用する",
    };

    public static readonly LocalizedString TableHeaderMob = new()
    {
        En = "Mob",
        Ja = "モブ",
    };

    public static readonly LocalizedString TableHeaderTime = new()
    {
        En = "Time",
        Ja = "経過時間",
    };

    public static readonly LocalizedString TableButtonTeleport = new()
    {
        En = "Teleport",
        Ja = "テレポ",
    };

    public static readonly LocalizedString TableButtonOpenMap = new()
    {
        En = "Open map",
        Ja = "マップを開く",
    };

    public static readonly LocalizedString TeleportingMessage = new()
    {
        En = "Teleporting to \"{0}\"...",
        Ja = "「{0}」にテレポしています...",
    };

    public static readonly LocalizedString AetheryteLinkInChatPluginNotInstalled = new()
    {
        En = "Divination.AetheryteLinkInChat plugin is not installed.",
        Ja = "Divination.AetheryteLinkInChat プラグインがインストールされていません。",
    };

    public static readonly LocalizedString GameExpansionARelmReborn = new()
    {
        En = "[2.x] A Relm Reborn",
        Ja = "[2.x] 新生エオルゼア",
    };

    public static readonly LocalizedString GameExpansionHeavensward = new()
    {
        En = "[3.x] Heavensward",
        Ja = "[3.x] 蒼天のイシュガルド",
    };

    public static readonly LocalizedString GameExpansionStormblood = new()
    {
        En = "[4.x] Stormblood",
        Ja = "[4.x] 紅蓮のリベレーター",
    };

    public static readonly LocalizedString GameExpansionShadowbringers = new()
    {
        En = "[5.x] Shadowbringers",
        Ja = "[5.x] 漆黒のヴィランズ",
    };

    public static readonly LocalizedString GameExpansionEndwalker = new()
    {
        En = "[6.x] Endwalker",
        Ja = "[6.x] 暁月のフィナーレ",
    };

    public static readonly LocalizedString GameExpansionDawntrail = new()
    {
        En = "[7.x] Dawntrail",
        Ja = "[7.x] 黄金のレガシー",
    };

    public static readonly LocalizedString JurisdictionNone = new()
    {
        En = "None",
        Ja = "なし",
    };

    public static readonly LocalizedString JurisdictionWorld = new()
    {
        En = "World",
        Ja = "ワールド",
    };

    public static readonly LocalizedString JurisdictionDataCenter = new()
    {
        En = "Data Center (logical DC)",
        Ja = "データセンター (論理DC)",
    };

    public static readonly LocalizedString JurisdictionRegion = new()
    {
        En = "Region (physical DC)",
        Ja = "リージョン (物理DC)",
    };

    public static readonly LocalizedString JurisdictionTravelable = new()
    {
        En = "Region + Travelable DC",
        Ja = "リージョン + データセンタートラベル可能なDC",
    };

    public static readonly LocalizedString JurisdictionAll = new()
    {
        En = "All",
        Ja = "すべて",
    };

    public static readonly LocalizedString PluginStatus = new()
    {
        En = "Status: {0}",
        Ja = "プラグインの状態: {0}",
    };

    public static readonly LocalizedString PluginStatusNotReady = new()
    {
        En = "Faloop account not set",
        Ja = "Faloop アカウントが設定されていません",
    };

    public static readonly LocalizedString PluginStatusDisconnected = new()
    {
        En = "disconnected from Faloop",
        Ja = "Faloop から切断されました",
    };

    public static readonly LocalizedString PluginStatusConnected = new()
    {
        En = "connected to Faloop",
        Ja = "Faloop に接続しました",
    };
}
