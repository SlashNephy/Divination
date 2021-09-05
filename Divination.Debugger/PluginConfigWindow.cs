using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.ClientState.Objects.Types;
using ImGuiNET;

namespace Divination.Debugger
{
    public class PluginConfigWindow : ConfigWindow<PluginConfig>
    {
        public override void Draw()
        {
            if (ImGui.Begin($"{DebuggerPlugin.Instance.Name} 設定", ref IsOpen, ImGuiWindowFlags.HorizontalScrollbar))
            {
                if (ImGui.BeginTabBar($"#{DebuggerPlugin.Instance.Name}-tabs"))
                {
                    CreateVariablesTab();
                    CreateConfigTab();

                    ImGui.EndTabBar();
                }

                ImGui.Separator();

                if (ImGui.Button("Save & Close"))
                {
                    IsOpen = false;

                    DebuggerPlugin.Instance.Dalamud.PluginInterface.SavePluginConfig(Config);
                    DebuggerPlugin.Instance.Logger.Information("Config saved");
                }

                ImGui.End();
            }
        }

        private void CreateVariablesTab()
        {
            if (ImGui.BeginTabItem("Variables"))
            {
                if (ImGui.CollapsingHeader("LocalPlayer"))
                {
                    ImGui.Indent();
                    DrawLocalPlayerTest();
                    ImGui.Unindent();
                }

                if (ImGui.CollapsingHeader("CurrentTarget"))
                {
                    ImGui.Indent();
                    DrawTargetTest();
                    ImGui.Unindent();
                }

                if (ImGui.CollapsingHeader("Process.GetCurrentProcess().Threads"))
                {
                    ImGui.Indent();
                    DrawThreadTest();
                    ImGui.Unindent();
                }

                ImGui.EndTabItem();
            }
        }

        private static int _filtered = -1;

        private void DrawLocalPlayerTest()
        {
            var player = DebuggerPlugin.Instance.Dalamud.ClientState.LocalPlayer;
            if (player == null)
            {
                return;
            }

            ImGui.InputInt("Filter", ref _filtered);

            var raw = new byte[0x19B0];
            Marshal.Copy(player.Address, raw, 0, raw.Length);
            var (l1, l2) = Transform(raw);

            ImGui.Columns(2);
            ImGui.Separator();

            ImGui.Text("i"); ImGui.NextColumn();
            ImGui.Text("Value"); ImGui.NextColumn();
            ImGui.Separator();

            foreach (var (i, value) in raw.Select((x, i) => (i, x)))
            {
                if (_filtered < 0 || _filtered == value)
                {
                    ImGui.Text($"0x{i:X4} ({i})"); ImGui.NextColumn();
                    ImGui.Text($"{value}"); ImGui.NextColumn();
                }
            }

            foreach (var (i, value) in l1.Select((x, i) => (i, x)))
            {
                if (_filtered < 0 || _filtered == value)
                {
                    ImGui.Text($"0x{i * 2:X4} ({i * 2})"); ImGui.NextColumn();
                    ImGui.Text($"{value}"); ImGui.NextColumn();
                }
            }

            foreach (var (i, value) in l2.Select((x, i) => (i, x)))
            {
                if (_filtered < 0 || _filtered == value)
                {
                    ImGui.Text($"0x{i * 4:X4}  ({i * 4})"); ImGui.NextColumn();
                    ImGui.Text($"{value}"); ImGui.NextColumn();
                }
            }

            ImGui.Columns(1);
        }

        private static (ushort[], uint[]) Transform(byte[] source)
        {
            var l1 = new ushort[source.Length / 2];
            for (var i = 0; i < l1.Length; i++)
            {
                l1[i] = BitConverter.ToUInt16(source, i * 2);
            }

            var l2 = new uint[source.Length / 4];
            for (var i = 0; i < l2.Length; i++)
            {
                l2[i] = BitConverter.ToUInt32(source, i * 4);
            }

            return (l1, l2);
        }

        private void DrawTargetTest()
        {
            var target = DebuggerPlugin.Instance.Dalamud.TargetManager.Target;
            if (target is Character character)
            {
                ImGui.Columns(2);
                ImGui.Separator();

                ImGui.NextColumn();
                ImGui.Text("Value"); ImGui.NextColumn();
                ImGui.Separator();

                ImGui.Text("Name"); ImGui.NextColumn();
                ImGui.Text(character.Name.TextValue); ImGui.NextColumn();

                ImGui.Separator();
                ImGui.Columns(1);
            }
            else
            {
                ImGui.Text("No Target Detected.");
            }
        }

        private void DrawThreadTest()
        {
            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            {
                ImGui.Text($"ID = {thread.Id}");
                ImGui.Indent();
                ImGui.Text($"State = {thread.ThreadState} ({(thread.ThreadState == ThreadState.Wait ? thread.WaitReason.ToString() : "N/A")})");
                ImGui.Unindent();
            }
        }

        private void CreateConfigTab()
        {
            if (ImGui.BeginTabItem("Config"))
            {
                ImGuiEx.CheckboxConfig("冗長なチャットログを表示する", ref Config.EnableVerboseChatLog,
                    "SeString としてパースされた, 冗長なチャットメッセージの構造を Debug Console に表示します。");

                ImGui.EndTabItem();
            }
        }
    }
}
