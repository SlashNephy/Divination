using Dalamud.Divination.Common.Api.Localize;

namespace Divination.AetheryteLinkInChat;

public static class Localization
{
    public static readonly LocalizedString TeleportingMessage = new()
    {
        En = "Teleporting to \"{0}\"...",
        Ja = "「{0}」にテレポしています...",
    };

    public static readonly LocalizedString QueueTeleportMessage = new()
    {
        En = "Currently unable to execute teleport. Teleport to \"{0}\" has been added to the queue.",
        Ja = "現在テレポを実行できません。「{0}」へのテレポをキューに追加しました。",
    };

    public static readonly LocalizedString QueuedTeleportingMessage = new()
    {
        En = "Teleporting to \"{0}\" (queued) ...",
        Ja = "キューに追加された「{0}」にテレポしています...",
    };

    public static readonly LocalizedString ConfigWindowTitle = new()
    {
        En = "{0} Config",
        Ja = "{0} 設定",
    };

    public static readonly LocalizedString AllowTeleportQueueing = new()
    {
        En = "Allow Teleport Queueing",
        Ja = "テレポートをキュー可能にする",
    };

    public static readonly LocalizedString QueueTeleportDelay = new()
    {
        En = "Queued Teleport Delay (ms)",
        Ja = "キューしたテレポートを実行するまでの遅延 (ms)",
    };

    public static readonly LocalizedString QueuedTeleportDescription = new()
    {
        En =
            "It can queue a teleportation when it is not possible to execute a teleportation, e.g. you are in combat.\nAfter teleporting becomes possible, the teleportation is executed after the delay set here.\nThis is necessary to prove that you are not cheating!",
        Ja = "戦闘中などでテレポが実行できないときはテレポをキューします。\nテレポが可能になったあと、ここで設定された遅延を挟んでからテレポを実行します。これはあなたが不正行為を行っていないと証明するために必要です！",
    };

    public static readonly LocalizedString ConsiderTeleportsToOtherWorlds = new()
    {
        En = "Consider Teleportation to Other Worlds in Route Calculation",
        Ja = "他ワールドへのテレポを経路計算に考慮する",
    };

    public static readonly LocalizedString ConsiderTeleportsToOtherWorldsDescription = new()
    {
        En =
            "When enabled, the Grand Company Aetheryte will be prepended to the best path\nif a destination world is different from the player's current world.",
        Ja = "有効にすると、目的地のワールドがプレイヤーの現在のワールドと異なる場合に、\n三国エーテライトを最短経路の先頭に付加します。",
    };

    public static readonly LocalizedString ConsiderTeleportsToOtherWorldsDisclaimer = new()
    {
        En = "This function simply detects the world name from the chat message.\nAs such, it may cause malfunctions.",
        Ja = "この機能は、単にワールド名をチャットメッセージから検出しているだけです。\nそのため、誤作動を起こす場合もあります。",
    };

    public static readonly LocalizedString PreferredGrandCompanyAetheryte = new()
    {
        En = "Preferred Grand Company Aetheryte",
        Ja = "優先する三国エーテライト",
    };

    public static readonly LocalizedString PreferredGrandCompanyAetheryteDescription = new()
    {
        En = "You can select the preferred Grand Company Aetheryte\nto use for teleporting to other worlds, for example.",
        Ja = "別ワールドにテレポするときなどに使用する、\n優先する三国エーテライトを選択できます。",
    };

    public static readonly LocalizedString SaveConfigButton = new()
    {
        En = "Save & Close",
        Ja = "保存して閉じる",
    };

    public static readonly LocalizedString TeleportGcHelpMessage = new()
    {
        En = "Teleports you to Grand Company Aetheryte. Useful for teleporting to other worlds. The aetheryte can be changed from plugin config.",
        Ja = "三国のエーテライトにテレポします。ワールド間テレポに便利です。優先する三国エーテライトは設定から変更できます。",
    };
}
