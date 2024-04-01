using System.Diagnostics;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using ImGuiNET;

namespace Divination.DiscordIntegration;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin($"{DiscordIntegration.Instance.Name} 設定", ref IsOpen, ImGuiWindowFlags.HorizontalScrollbar))
        {
            if (ImGui.BeginTabBar($"#{DiscordIntegration.Instance.Name}-tabs"))
            {
                CreateRichPresenceConfigTab();
                CreateVariablesConfigTab();

                ImGui.EndTabBar();
            }

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;

                DiscordIntegration.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                DalamudLog.Log.Information("Config saved");
            }

            ImGui.End();
        }
    }

    private void CreateRichPresenceConfigTab()
    {
        if (ImGui.BeginTabItem("Rich Presence"))
        {
            ImGui.Text("Discord に表示される Rich Presence (詳細なプレイ中の情報) をカスタマイズできます。");
            ImGui.Text("以下のテンプレートは, ゲーム内の情報を表示するための様々な変数を使用できます。");
            ImGui.Text("変数の一覧や実際のテンプレートの値は, 「変数 & テンプレート」タブで確認できます。");

            if (ImGui.Button("Rich Presence の詳細"))
            {
                Process.Start("https://discord.com/developers/docs/rich-presence/how-to#updating-presence");
            }

            ImGui.Separator();

            if (ImGui.CollapsingHeader("Details"))
            {
                ImGuiEx.TextConfig("通常時", ref Config.DetailsFormat, 128, "通常時の Details のテンプレートを設定します。");
                ImGuiEx.TextConfig("オンライン中", ref Config.DetailsInOnlineFormat, 128, "オンラインステータスが「オンライン」 かつ 非戦闘状態に表示される Details のテンプレートを設定します。");
                ImGuiEx.TextConfig("コンテンツ中", ref Config.DetailsInDutyFormat, 128, "コンテンツ中 かつ 非戦闘状態に表示される Details のテンプレートを設定します。");
                ImGuiEx.TextConfig("戦闘状態", ref Config.DetailsInCombatFormat, 128, "戦闘状態時に表示される Details のテンプレートを設定します。");

                ImGuiEx.CheckboxConfig("ターゲット時のみ戦闘中の判定にする",
                    ref Config.RequireTargetingOnCombat,
                    "ターゲットしているときのみ, 戦闘状態となるように判定を変更します。",
                    "戦闘中のテンプレートに {target} などを使っている場合に, 空白の {target} が出力されることを防止できます。");
            }

            ImGui.Separator();

            ImGuiEx.TextConfig("State", ref Config.StateFormat, 128, "State のテンプレートを設定します。");

            ImGui.Separator();

            if (ImGui.CollapsingHeader("Small Image"))
            {
                ImGuiEx.CheckboxConfig("ジョブアイコンを設定する", ref Config.ShowJobSmallImage, "Small Image としてジョブアイコンを表示します。");

                ImGuiEx.TextConfig("Small Image Text",
                    ref Config.SmallImageTextFormat,
                    128,
                    "Small Image Text のテンプレートを設定します。",
                    "この値は, Small Image をホバーした時に表示されます。");
            }

            ImGui.Separator();

            if (ImGui.CollapsingHeader("Large Image"))
            {
                ImGuiEx.CheckboxConfig("ローディング画像を設定する", ref Config.ShowLoadingLargeImage, "Large Image として地域ごとのローディング画像 (ロード中の背景画像) を表示します。");

                ImGuiEx.TextConfig("Large Image Text",
                    ref Config.LargeImageTextFormat,
                    128,
                    "Large Image Text のテンプレートを設定します。",
                    "このテンプレートは, Large Image をホバーした時に表示されます。");
            }

            ImGui.Separator();

            ImGuiEx.CheckboxConfig("エリア移動時に経過時間をリセットする",
                ref Config.ResetTimerOnAreaChange,
                "エリアチェンジ時に経過時間のタイマーをリセットします。",
                "ただし, コンテンツ中はコンテンツ退出までタイマーはリセットされません。");

            ImGui.EndTabItem();
        }
    }

    private void CreateVariablesConfigTab()
    {
        if (ImGui.BeginTabItem("変数 & テンプレート"))
        {
            ImGui.Text("各変数とテンプレートの値を確認できます。");
            ImGui.Text("対応しているプラグインからの変数 (IPC 変数) やテンプレートの値 (IPC テンプレート) を使用することもできます。");

            ImGui.Separator();

            var instance = Formatter.Instance;
            CreateNormalVariablesCollapsingHeader(instance);
            CreateNormalTemplatesCollapsingHeader(instance);

            CreateIpcVariablesCollapsingHeader();
            CreateIpcTemplatesCollapsingHeader(instance);

            ImGui.EndTabItem();
        }
    }

    private static void CreateNormalVariablesCollapsingHeader(Formatter? instance)
    {
        if (ImGui.CollapsingHeader("変数"))
        {
            ImGui.Columns(3);
            ImGui.Separator();
            ImGui.Text("変数名");
            ImGui.NextColumn();
            ImGui.Text("値");
            ImGui.NextColumn();
            ImGui.Text("説明");
            ImGui.NextColumn();
            ImGui.Separator();

            ImGui.Text("{fc}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Fc ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("FC タグ名");
            ImGui.NextColumn();

            ImGui.Text("{world}");
            ImGui.NextColumn();
            ImGui.Text(instance?.World ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のワールド名");
            ImGui.NextColumn();

            ImGui.Text("{home_world}");
            ImGui.NextColumn();
            ImGui.Text(instance?.HomeWorld ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("ホームワールド名");
            ImGui.NextColumn();

            ImGui.Text("{level}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Level ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のクラス・ジョブのレベル");
            ImGui.NextColumn();

            ImGui.Text("{name}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Name ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("キャラクター名");
            ImGui.NextColumn();

            ImGui.Text("{job}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Job ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のクラス・ジョブの略称");
            ImGui.NextColumn();

            ImGui.Text("{job_name}");
            ImGui.NextColumn();
            ImGui.Text(instance?.JobName ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のクラス・ジョブ");
            ImGui.NextColumn();

            ImGui.Text("{x}");
            ImGui.NextColumn();
            ImGui.Text(instance?.X ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("x 座標");
            ImGui.NextColumn();

            ImGui.Text("{y}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Y ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("y 座標");
            ImGui.NextColumn();

            ImGui.Text("{z}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Z ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("z 座標");
            ImGui.NextColumn();

            ImGui.Text("{hp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Hp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の HP");
            ImGui.NextColumn();

            ImGui.Text("{hpp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Hpp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の HP 割合");
            ImGui.NextColumn();

            ImGui.Text("{mp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Mp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の MP");
            ImGui.NextColumn();

            ImGui.Text("{mpp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Mpp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の MP 割合");
            ImGui.NextColumn();

            ImGui.Text("{cp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Cp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の CP");
            ImGui.NextColumn();

            ImGui.Text("{cpp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Cpp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の CP 割合");
            ImGui.NextColumn();

            ImGui.Text("{gp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Gp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の GP");
            ImGui.NextColumn();

            ImGui.Text("{gpp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Gpp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の GP 割合");
            ImGui.NextColumn();

            ImGui.Text("{territory}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Territory ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のエリアの TerritoryType");
            ImGui.NextColumn();

            ImGui.Text("{place}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Place ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のエリアの PlaceName");
            ImGui.NextColumn();

            ImGui.Text("{region}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Region ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のエリアの PlaceRegion");
            ImGui.NextColumn();

            ImGui.Text("{zone}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Zone ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のエリアの PlaceZone");
            ImGui.NextColumn();

            ImGui.Text("{target}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Target ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のターゲット名");
            ImGui.NextColumn();

            ImGui.Text("{thp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Thp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("ターゲットの現在の HP");
            ImGui.NextColumn();

            ImGui.Text("{thpp}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Thpp ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("ターゲットの現在の HP 割合");
            ImGui.NextColumn();

            ImGui.Text("{party}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Party ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のパーティ構成");
            ImGui.NextColumn();

            ImGui.Text("{duty}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Duty ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在参加中のコンテンツ名");
            ImGui.NextColumn();

            ImGui.Text("{status}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Status ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在のオンラインステータス");
            ImGui.NextColumn();

            ImGui.Text("{title}");
            ImGui.NextColumn();
            ImGui.Text(instance?.Title ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text("現在の称号");
            ImGui.NextColumn();

            ImGui.Columns(1);
        }
    }

    private static void CreateNormalTemplatesCollapsingHeader(Formatter? instance)
    {
        if (ImGui.CollapsingHeader("テンプレート"))
        {
            ImGui.Columns(3);
            ImGui.Separator();
            ImGui.Text("テンプレート名");
            ImGui.NextColumn();
            ImGui.Text("テンプレート");
            ImGui.NextColumn();
            ImGui.Text("評価された値");
            ImGui.NextColumn();
            ImGui.Separator();

            ImGui.Text("Details");
            ImGui.NextColumn();
            ImGui.Text(instance?.GetTemplate("details") ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text(instance?.Render(instance.GetTemplate("details")) ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("State");
            ImGui.NextColumn();
            ImGui.Text(instance?.GetTemplate("state") ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text(instance?.Render(instance.GetTemplate("state")) ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("Small Image Key");
            ImGui.NextColumn();
            ImGui.NextColumn();
            ImGui.Text(instance?.SmallImageKey ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("Small Image Text");
            ImGui.NextColumn();
            ImGui.Text(instance?.GetTemplate("small_image_text") ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text(instance?.Render(instance.GetTemplate("small_image_text")) ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("Large Image Key");
            ImGui.NextColumn();
            ImGui.NextColumn();
            ImGui.Text(instance?.LargeImageKey ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("Large Image Text");
            ImGui.NextColumn();
            ImGui.Text(instance?.GetTemplate("large_image_text") ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text(instance?.Render(instance.GetTemplate("large_image_text")) ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Text("Custom Status");
            ImGui.NextColumn();
            ImGui.Text(instance?.GetTemplate("custom_status") ?? string.Empty);
            ImGui.NextColumn();
            ImGui.Text(instance?.Render(instance.GetTemplate("custom_status")) ?? string.Empty);
            ImGui.NextColumn();

            ImGui.Columns(1);
        }
    }

    private static void CreateIpcVariablesCollapsingHeader()
    {
        if (ImGui.CollapsingHeader("IPC 変数"))
        {
            ImGui.Columns(3);
            ImGui.Separator();
            ImGui.Text("変数名");
            ImGui.NextColumn();
            ImGui.Text("値");
            ImGui.NextColumn();
            ImGui.Text("プラグイン");
            ImGui.NextColumn();
            ImGui.Separator();

            lock (Formatter.IpcVariables)
            {
                foreach (var (name, (variable, target, _)) in Formatter.IpcVariables)
                {
                    ImGui.Text(name);
                    ImGui.NextColumn();
                    ImGui.Text(variable);
                    ImGui.NextColumn();
                    ImGui.Text(target);
                    ImGui.NextColumn();
                }
            }

            ImGui.Columns(1);
        }
    }

    private static void CreateIpcTemplatesCollapsingHeader(Formatter? instance)
    {
        if (ImGui.CollapsingHeader("IPC テンプレート"))
        {
            ImGui.Columns(3);
            ImGui.Separator();
            ImGui.Text("テンプレート名");
            ImGui.NextColumn();
            ImGui.Text("テンプレート");
            ImGui.NextColumn();
            ImGui.Text("評価された値");
            ImGui.NextColumn();
            ImGui.Separator();

            lock (Formatter.IpcTemplates)
            {
                foreach (var (name, (template, target, _)) in Formatter.IpcTemplates)
                {
                    ImGui.Text($"{name} ({target})");
                    ImGui.NextColumn();
                    ImGui.Text(template);
                    ImGui.NextColumn();
                    ImGui.Text(instance?.Render(template) ?? string.Empty);
                    ImGui.NextColumn();
                }
            }

            ImGui.Columns(1);
        }
    }
}
