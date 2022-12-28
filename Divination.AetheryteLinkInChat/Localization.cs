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
        En = "It can queue when teleport is unavailable, e.g. in combat.\nThen your teleport will be executed after the Delay.",
        Ja = "戦闘中などでテレポが実行できないときはテレポをキューします。\nテレポが可能になったあと、遅延を挟んでからテレポを実行します。",
    };

    public static readonly LocalizedString PreferredGrandCompanyAetheryte = new()
    {
        En = "Preferred Grand Company Aetheryte",
        Ja = "優先する三国エーテライト",
    };

    public static readonly LocalizedString PreferredGrandCompanyAetheryteDescription = new()
    {
        En = "You can specify the Aetheryte to be used when teleporting to another world.\nIn the case of coordinate information for another world, this Aetheryte will be displayed at the beginning of the route.",
        Ja = "別ワールドにテレポするときに使用するエーテライトを指定できます。\n別ワールドの座標情報の場合、このエーテライトが経路の先頭に表示されるようになります。",
    };

    public static readonly LocalizedString SaveConfigButton = new()
    {
        En = "Save & Close",
        Ja = "保存して閉じる",
    };

    public static readonly LocalizedString TeleportGcHelpMessage = new()
    {
        En = "Teleports you to Grand Company Aetheryte. Useful for teleporting to another world. The aetheryte can be changed from plugin config.",
        Ja = "三国のエーテライトにテレポします。ワールド間テレポに便利です。優先する三国エーテライトは設定から変更できます。",
    };
}
