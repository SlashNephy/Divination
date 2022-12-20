namespace Divination.AetheryteLinkInChat.Localize;

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
        En = "Teleporting to \"{0}\"...",
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
        En = "It can queue when teleport is unavailable, e.g. in combat.\nThen your teleport will be executed after the Delay.",
        Ja = "戦闘中などでテレポが実行できないときはテレポをキューします。\nテレポが可能になったあと、遅延を挟んでからテレポを実行します。",
    };

    public static readonly LocalizedString SaveConfigButton = new()
    {
        En = "Save & Close",
        Ja = "保存して閉じる",
    };
}
