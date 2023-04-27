using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.Text;
using Dalamud.Logging;
using ImGuiNET;

namespace Divination.SseClient;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{SseClient.Instance.Name} 設定", ref IsOpen, ImGuiWindowFlags.HorizontalScrollbar))
        {
            if (ImGui.BeginTabBar($"#{SseClient.Instance.Name}-tabs"))
            {
                CreateEndpointConfigTab();
                CreateReceiveConfigTab();
                CreateSendConfigTab();
                CreateFilterConfigTab();

                ImGui.EndTabBar();
            }

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;

                SseClient.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                PluginLog.Information("Config saved.");

                SseClient.IsConfigChanged = true;
                SseClient.ConnectToSse();
            }

            ImGui.End();
        }
    }

    private static readonly string[] Types = Enum.GetNames(typeof(XivChatType)).ToArray();

    private void CreateEndpointConfigTab()
    {
        if (ImGui.BeginTabItem("接続先"))
        {
            ImGui.Text($"現在の状態: {(SseClient.IsDisconnected ? "未接続" : "接続中")}");

            ImGui.Separator();

            ImGui.Text("SseClient を使用する前に SseServer の接続先を設定する必要があります。");

            ImGui.Separator();

            ImGuiEx.TextConfig("SseServer URL", ref Config.EndpointUrl, 256, "接続する SseServer の URL を入力します。");
            ImGuiEx.TextConfig("SseServer Token", ref Config.Token, 64, "SseServer の管理者から提示された認証用のトークンを入力します。");

            ImGui.EndTabItem();
        }
    }

    private void CreateReceiveConfigTab()
    {
        if (ImGui.BeginTabItem("受信"))
        {
            ImGui.Text("FFXIV のゲーム内チャットに受信したいデータを選択します。");

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("非通知モードを有効にする", ref Config.EnableDoNotDisturb,
                "すべての SSE メッセージを非表示にします。");

            ImGui.Separator();

            ImGui.Checkbox("Horoscope Discord", ref Config.ReceiveDiscordMessages);
            ImGui.Combo("##receive_discord", ref Config.DiscordMessagesTypeIndex, Types, Types.Length);
            ImGui.Indent();
            ImGui.Text("Horoscope Discord からメッセージを表示します。");
            ImGui.Unindent();

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("ゴールドソーサー", ref Config.ReceiveGateAnnouncements,
                "G.A.T.E. の開催情報やジャンボくじテンダーのアナウンスなどを表示します。");

            ImGui.Separator();

            if (ImGui.CollapsingHeader("モブハント"))
            {
                ImGui.Text("モブハント関連のチャットを表示します。");

                ImGui.Separator();

                ImGui.Checkbox("モブハント LS", ref Config.ReceiveMobHuntLsMessages);
                ImGui.Combo("##receive_mobhunt_ls", ref Config.MobHuntLsMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("モブハント LS からのメッセージを表示します。");
                ImGui.Unindent();

                ImGui.Checkbox("モブハント CWLS", ref Config.ReceiveMobHuntCwlsMessages);
                ImGui.Combo("##receive_mobhunt_cwls", ref Config.MobHuntCwlsMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("モブハント CWLS からのメッセージを表示します。");
                ImGui.Unindent();

                ImGuiEx.CheckboxConfig("LS1/CWLS メッセージをフィルタする", ref Config.FilterMobHuntLsMessages,
                    "\"ありがとー！\" のようなモブハントに関係のないメッセージを非表示にします。");

                ImGuiEx.CheckboxConfig("同一のマップリンクをフィルタする", ref Config.FilterSameMapLinks,
                    "全く同じ座標のマップリンクを非表示にします。");

                ImGuiEx.CheckboxConfig("Sonar の Bモブ通知を非表示にする", ref Config.FilterSonarRankBMessages,
                    "Sonar の Bモブの通知メッセージを非表示にします。");

                ImGui.Separator();

                ImGui.Checkbox("モブハント Discord", ref Config.ReceiveMobHuntDiscordMessages);
                ImGui.Combo("##receive_mobhunt_discord", ref Config.MobHuntDiscordMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("モブハント Discord からのメッセージを表示します。");
                ImGui.Unindent();

                ImGui.Separator();

                ImGui.Checkbox("Shout", ref Config.ReceiveMobHuntShoutMessages);
                ImGui.Combo("##receive_shout", ref Config.MobHuntShoutMessagesTypeIndex, Types, Types.Length);
                ImGui.Checkbox("Yell", ref Config.ReceiveMobHuntYellMessages);
                ImGui.Combo("##receive_yell", ref Config.MobHuntYellMessagesTypeIndex, Types, Types.Length);
                ImGui.Checkbox("Say", ref Config.ReceiveMobHuntSayMessages);
                ImGui.Combo("##receive_say", ref Config.MobHuntSayMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("Shout/Yell/Say メッセージを表示します。");
                ImGui.Text("エウレカ内では [Eureka] というプレフィックスが付きます。");
                ImGui.Text("ボズヤ戦線内では [Bozja] というプレフィックスが付きます。");
                ImGui.Text("フィールドではワールド名が付きます。例えば Ifrit ワールドでは [Ifrit] というプレフィックスが付きます。");
                ImGui.Unindent();

                ImGuiEx.CheckboxConfig("SHOUT/Yell/Say メッセージをフィルタする", ref Config.FilterMobHuntShoutMessages,
                    "SHOUT/Yell/Say のメッセージをフィルタします。\"蘇生おねがいします\" や, 座標を含まないメッセージを非表示にします。");

                ImGui.Separator();

                ImGui.Checkbox("漆黒エリアの配下発生ログ", ref Config.ReceiveMobHuntSystemMessages);
                ImGui.Combo("##receive_mobhunt_system", ref Config.MobHuntSystemMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text($"「{MobHuntRankSsPopSystemMessage}」を表示にします。");
                ImGui.Unindent();

                ImGui.Separator();
                ImGuiEx.CheckboxConfig("アクティブなリスキーモブ情報を受信する", ref Config.ReceiveMobHuntActor,
                    "他ユーザーが送信したリスキーモブ情報を取得します。");
                ImGuiEx.CheckboxConfig("アクティブなリスキーモブ情報をチャットに表示する", ref Config.ShowMobHuntActorChat,
                    "アクティブなリスキーモブの名前と HP をチャットに表示します。");
                ImGuiEx.CheckboxConfig("アクティブなリスキーモブ情報のウィジェットを表示する", ref Config.ShowMobHuntActorWidget,
                    "アクティブなリスキーモブの名前と HP をウィジェットに表示します。");
            }

            ImGui.Separator();

            if (ImGui.CollapsingHeader("Faloop"))
            {
                ImGui.Text("Faloop (https://faloop.app) からのメッセージを表示します。");

                ImGui.Separator();

                ImGui.Checkbox("Sモブ 湧き情報", ref Config.ReceiveFaloopSpawnMessages);
                ImGui.Combo("##receive_faloop_spawn", ref Config.MobHuntFaloopSpawnMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("Sモブの湧き情報や誤報通知を表示します。");
                ImGui.Unindent();

                ImGui.Checkbox("Sモブ 討伐情報", ref Config.ReceiveFaloopKillMessages);
                ImGui.Combo("##receive_faloop_kill", ref Config.MobHuntFaloopKillMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("Sモブの討伐情報を表示します。");
                ImGui.Unindent();

                ImGuiEx.CheckboxConfig("古い討伐記録を非表示にする", ref Config.FilterOldMobKills,
                    "10分より前の討伐記録は非表示にします。");

                ImGui.Checkbox("Bモブ 目撃情報", ref Config.ReceiveFaloopSightMessages);
                ImGui.Combo("##receive_faloop_sight", ref Config.MobHuntFaloopSightMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("Bモブの目撃情報を表示します。");
                ImGui.Unindent();

                ImGui.Checkbox("システム", ref Config.ReceiveFaloopSystemMessages);
                ImGui.Combo("##receive_faloop_system", ref Config.MobHuntFaloopSystemMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("Faloop の障害情報や WebSockets の接続に関するメッセージを表示します。");
                ImGui.Unindent();
            }

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("SSE から受信したメッセージにアイコンを付与する", ref Config.ShowIconOnReceivedMessages,
                "送信者の名前にアイコンを付けます。",
                "ゲーム内のメッセージと SSE 経由で受信したメッセージの区別が付きやすくなります。");

            ImGui.EndTabItem();
        }
    }

    private void CreateSendConfigTab()
    {
        if (ImGui.BeginTabItem("送信"))
        {
            ImGui.Text("SseClient はクライアント間でチャットログを共有します。");
            ImGui.Text("ここで共有するデータを制限できます。");

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("FC チャット", ref Config.SendFcMessages,
                "FC チャットを共有します。",
                "共有されたチャットは Horoscope Discord の #general-vc に表示されます。");
            ImGui.Combo("##send_fc", ref Config.FCMessagesTypeIndex, Types, Types.Length);

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("ゴールドソーサー", ref Config.SendGateAnnouncements,
                "G.A.T.E. の開催情報やジャンボくじテンダーのアナウンスなどを共有します。");

            ImGui.Separator();

            if (ImGui.CollapsingHeader("モブハント"))
            {
                ImGui.Checkbox("LS チャット", ref Config.SendLs1Messages);
                ImGui.Combo("##send_ls", ref Config.LsMessagesTypeIndex, Types, Types.Length);
                ImGui.Checkbox("CWLS チャット", ref Config.SendCwls1Messages);
                ImGui.Combo("##send_cwls", ref Config.CwlsMessagesTypeIndex, Types, Types.Length);
                ImGui.Indent();
                ImGui.Text("LS/CWLS のメッセージを共有します。");
                ImGui.Text("送信されたメッセージはモブハント関連とみなされます。");
                ImGui.Unindent();

                ImGui.Separator();

                ImGui.Checkbox("SHOUT チャット", ref Config.SendShoutMessages);
                ImGui.Checkbox("Yell チャット", ref Config.SendYellMessages);
                ImGui.Checkbox("Say チャット", ref Config.SendSayMessages);
                ImGui.Indent();
                ImGui.Text("Shout/Yell/Say メッセージを共有します。");
                ImGui.Unindent();

                ImGui.Separator();

                ImGuiEx.CheckboxConfig("漆黒エリアの配下発生ログ", ref Config.SendMobHuntSystemMessages,
                    $"「{MobHuntRankSsPopSystemMessage}」を共有にします。");

                ImGui.Separator();

                ImGuiEx.CheckboxConfig("アクティブなリスキーモブ情報", ref Config.SendMobHuntActor,
                    "アクティブなリスキーモブの名前と HP を共有します。");
            }

            ImGui.EndTabItem();
        }
    }

    private void CreateFilterConfigTab()
    {
        if (ImGui.BeginTabItem("フィルタ"))
        {
            ImGui.Checkbox("重複メッセージ", ref Config.FilterSameMessageIn5Min);
            ImGui.Indent();
            ImGui.Text("5分以内に同一の送信者かつ重複したメッセージを受け取ると非表示にします。");
            ImGui.Text("フィルタ対象となっているチャンネルは以下の通りです。");
            var list = CheckDuplicationTypes
                .Append(Config.LsMessagesType)
                .Append(Config.CwlsMessagesType)
                .Distinct()
                .Where(x => Enum.IsDefined(typeof(XivChatType), x))
                .Select((v, i) => new { v, i })
                .GroupBy(x => x.i / 5)
                .Select(g => g.Select(x => x.v));
            foreach (var x in list)
            {
                ImGui.Text(string.Join(", ", x));
            }
            ImGui.Unindent();

            ImGuiEx.CheckboxConfig("Echo メッセージも重複チェック対象にする", ref Config.FilterEchoDuplication,
                "重複した Echo メッセージを非表示にします。");

            ImGuiEx.CheckboxConfig("システムメッセージ", ref Config.FilterNoticeMessages,
                "ワールド間テレポ後の「Welcome to Valefor!」のようなメッセージを非表示にします。");

            ImGuiEx.CheckboxConfig("ビギナーチャンネルの参加メッセージ", ref Config.FilterNoviceNetworkMessages,
                "ビギナーチャンネルの参加メッセージを非表示にします。");

            ImGuiEx.CheckboxConfig("Debug メッセージ", ref Config.FilterDebugMessages,
                "Debug メッセージを非表示にします。",
                "通常 Debug メッセージは他の Dalamud プラグインによって書き込まれます。");

            ImGuiEx.CheckboxConfig("Debug を Echo に移動する", ref Config.MoveDebugMessagesToEcho,
                "フィルタした Debug メッセージを Echo に表示にします。",
                "Echo として表示することで, ゲーム内の「ログフィルタ」が機能するようになります。");

            ImGui.EndTabItem();
        }
    }
}
