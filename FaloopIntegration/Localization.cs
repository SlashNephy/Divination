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
        En = "{0} Config",
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

    public static readonly LocalizedString PerRank = new()
    {
        En = "Per Rank Preferences",
        Ja = "モブの階級ごとの設定",
    };

    public static readonly LocalizedString RankS = new()
    {
        En = "Rank S",
        Ja = "Sモブ",
    };

    public static readonly LocalizedString RankFate = new()
    {
        En = "Special Fate",
        Ja = "特殊 F.A.T.E.",
    };

    public static readonly LocalizedString ReportChannel = new()
    {
        En = "Channel",
        Ja = "通知先",
    };

    public static readonly LocalizedString ReportConditions = new()
    {
        En = "Report Conditions",
        Ja = "通知条件",
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
        En = "For example, if you specify \"World,\" you will receive only notifications about your home world.",
        Ja = "例えば「World」を指定すると、ホームワールドに関する通知のみを受け取ります。",
    };

    public static readonly LocalizedString ReportExpansions = new()
    {
        En = "Expansions",
        Ja = "拡張パッチ",
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

    public static readonly LocalizedString ReportIgnoreOrphanDeathReportDescription = new()
    {
        En = "If enabled, the plugin will ignore reports of mob kills for which it has not received a spawn notification.",
        Ja = "有効にすると、湧き通知を受け取っていないモブの討伐報告を無視します。",
    };

    public static readonly LocalizedString ActiveMob = new()
    {
        En = "Active Mobs",
        Ja = "現在のモブ",
    };

    public static readonly LocalizedString EnableActiveMobUi = new()
    {
        En = "Enable Active Mobs UI",
        Ja = "「現在のモブ」パネルを表示",
    };

    public static readonly LocalizedString EnableSimpleReports = new()
    {
        En = "Enable simplified, condensed reports in chat",
        Ja = "",
    };
}
