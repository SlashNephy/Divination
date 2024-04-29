using Dalamud.Divination.Common.Api.Localize;

namespace Divination.AetheryteLinkInChat;

public static class Localization
{
    public static readonly LocalizedString TeleportingMessage = new()
    {
        En = "Teleporting to \"{0}\"...",
        Ja = "「{0}」にテレポしています...",
    };

    public static readonly LocalizedString TeleportingQuestMessage = new()
    {
        En = "Teleport to \"{0}\"",
        Ja = "「{0}」にテレポ",
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

    public static readonly LocalizedString QueuedTeleportingQuestMessage = new()
    {
        En = "Teleport to \"{0}\" (queued)",
        Ja = "キューに追加された「{0}」にテレポ",
    };

    public static readonly LocalizedString ConfigWindowTitle = new()
    {
        En = "{0} Config",
        Ja = "{0} 設定",
    };

    public static readonly LocalizedString AllowTeleportQueueing = new()
    {
        En = "Allow Teleport Queueing",
        Ja = "テレポを予約可能にする",
    };

    public static readonly LocalizedString QueueTeleportDelay = new()
    {
        En = "Queued Teleport Delay (ms)",
        Ja = "予約したテレポを実行するまでの遅延 (ms)",
    };

    public static readonly LocalizedString QueuedTeleportDescription = new()
    {
        En =
            "It can queue a teleportation when it is not possible to execute a teleportation, e.g. you are in combat.\nAfter teleporting becomes possible, the teleportation is executed after the delay set here.\nThis is necessary to prove that you are not cheating!",
        Ja = "戦闘中などでテレポが実行できないときはテレポをキューに追加します。\nテレポが可能になったあと、ここで設定された遅延を挟んでからテレポを実行します。\nこれはあなたが不正行為を行っていないと証明するために必要です！",
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

    public static readonly LocalizedString EnableLifestreamIntegration = new()
    {
        En = "Enable Lifestream Integration",
        Ja = "Lifestream との連携機能を有効にする",
    };

    public static readonly LocalizedString EnableLifestreamIntegrationDescription = new()
    {
        En = "If enabled, teleporting to destinations will be automated, including world traveling and aethernet transfers.\nLifestream plugin required!",
        Ja = "有効にすると、ワールド間テレポや都市内エーテライトの移動を含め、目的地へのテレポが自動化されます。\nこの機能を使用するには、Lifestream プラグインが必要です！",
    };

    public static readonly LocalizedString LifestreamUnavailable = new()
    {
        En = "Lifestream plugin is not available. Please install it.",
        Ja = "Lifestream がインストールされていません。",
    };

    public static readonly LocalizedString DisplayLineBreak = new()
    {
        En = "Line break before teleport links",
        Ja = "テレポリンクの前に改行を入れる",
    };

    public static readonly LocalizedString EnableChatNotificationOnTeleport = new()
    {
        En = "Enable Chat Notification on Teleport",
        Ja = "テレポ実行時にチャット通知を有効にする",
    };

    public static readonly LocalizedString EnableQuestNotificationOnTeleport = new()
    {
        En = "Enable Toast Notification on Teleport",
        Ja = "テレポ実行時にトースト通知を有効にする",
    };

    public static readonly LocalizedString IgnoredAetherytes = new()
    {
        En = "You can set a specific aetherite not to be used in route calculations.",
        Ja = "特定のエーテライトを経路計算で使用しないように設定できます。",
    };
}
