using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Divination.Common.Api.Ui;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Network;
using ImGuiNET;

namespace Divination.Debugger.Window;

public class PluginConfigWindow : ConfigWindow<PluginConfig>
{
    public override void Draw()
    {
        if (ImGui.Begin(DebuggerPlugin.Instance.Name, ref IsOpen, ImGuiWindowFlags.HorizontalScrollbar))
        {
            if (ImGui.BeginTabBar($"#{DebuggerPlugin.Instance.Name}-tabs"))
            {
                CreatePlayerAnalyzerTab();
                CreateNetworkAnalyzerTab();
                CreateConfigTab();

                ImGui.EndTabBar();
            }

            ImGui.Separator();

            if (ImGui.Button("Save & Close"))
            {
                IsOpen = false;
                Save();
            }

            ImGui.End();
        }
    }

    private readonly string[] dataTypes = Enum.GetNames(typeof(DataType)).ToArray();

    private void CreatePlayerAnalyzerTab()
    {
        if (ImGui.BeginTabItem("Player"))
        {
            var target = DebuggerPlugin.Instance.Dalamud.TargetManager.Target as Character ?? DebuggerPlugin.Instance.Dalamud.ClientState.LocalPlayer;
            if (target == null)
            {
                ImGui.Text("Neither LocalPlayer nor Target is detected.");
                ImGui.EndTabItem();
                return;
            }

            ImGui.Text($"Source: {target.Name} ({(DebuggerPlugin.Instance.Dalamud.TargetManager.Target == null ? "LocalPlayer" : "Target")})");

            ImGui.Separator();

            ImGui.Combo("Data Type", ref Config.PlayerDataTypeIndex, dataTypes, dataTypes.Length);
            ImGui.Checkbox("Enable Filter", ref Config.PlayerEnableValueFilter);

            unsafe
            {
                fixed (long* ptr = &Config.PlayerFilterValue)
                {
                    ImGui.InputScalar("Filter Value", ImGuiDataType.S64, new IntPtr(ptr));
                }
            }

            ImGui.Separator();

            // https://github.com/aers/FFXIVClientStructs/blob/main/FFXIVClientStructs/FFXIV/Client/Game/Character/Character.cs
            var data = new byte[0x19F0];
            Marshal.Copy(target.Address, data, 0, data.Length);

            var viewer = new DataViewer(Config.PlayerDataType, data, Config.PlayerEnableValueFilter, Config.PlayerFilterValue);
            viewer.Draw();

            ImGui.EndTabItem();
        }
    }

    private NetworkContext? lastNetworkContext;
    private DataViewer? lastNetworkViewer;

    private void CreateNetworkAnalyzerTab()
    {
        if (ImGui.BeginTabItem("Network"))
        {
            ImGui.Checkbox("Enable Network Listener", ref Config.NetworkEnableListener);

            ImGui.Checkbox("Download", ref Config.NetworkListenDownload);
            ImGui.SameLine();
            ImGui.Checkbox("Upload", ref Config.NetworkListenUpload);

            if (ImGui.Button("Clear"))
            {
                lastNetworkContext = null;
                lastNetworkViewer = null;
            }

            ImGui.Separator();

            ImGui.Combo("Data Type", ref Config.NetworkDataTypeIndex, dataTypes, dataTypes.Length);
            ImGui.Checkbox("Enable Value Filter", ref Config.NetworkEnableValueFilter);

            unsafe
            {
                fixed (long* ptr = &Config.NetworkFilterValue)
                {
                    ImGui.InputScalar("Value", ImGuiDataType.S64, new IntPtr(ptr));
                }
            }

            ImGui.Checkbox("Enable Opcode Filter", ref Config.NetworkEnableOpcodeFilter);
            ImGui.InputInt("Opcode", ref Config.NetworkFilterOpcode);

            ImGui.Separator();

            var context = NetworkListener.LastContext;
            if (context != null)
            {
                var viewer = new DataViewer(Config.NetworkDataType, context.Data, Config.NetworkEnableValueFilter, Config.PlayerFilterValue);
                if (viewer.Any())
                {
                    lastNetworkContext = context;
                    lastNetworkViewer = viewer;
                }
                else
                {
                    context = lastNetworkContext;
                    viewer = lastNetworkViewer;
                }

                if (context != null && viewer != null)
                {
                    ImGui.Text($"Direction = {Enum.GetName(typeof(NetworkMessageDirection), context.Direction)}");
                    ImGui.Text($"Opcode = 0x{context.Opcode:X4}");

                    viewer.Draw();
                }
            }

            ImGui.EndTabItem();
        }
    }

    private void CreateConfigTab()
    {
        if (ImGui.BeginTabItem("Config"))
        {
            ImGuiEx.CheckboxConfig("起動時にウィンドウを開く", ref Config.OpenAtStart);

            ImGuiEx.CheckboxConfig("冗長なチャットログを表示する", ref Config.EnableVerboseChatLog,
                "SeString としてパースされた, 冗長なチャットメッセージの構造をログに表示します。");

            ImGui.EndTabItem();
        }
    }
}
