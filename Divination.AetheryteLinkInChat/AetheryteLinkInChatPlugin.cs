using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Data;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat
{
    public class AetheryteLinkInChatPlugin : IDalamudPlugin
    {
        public string Name => "Divination.AetheryteLinkInChat";

        // @formatter:off
        [PluginService] [RequiredVersion("1.0")] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] [RequiredVersion("1.0")] public static DataManager GameData { get; private set; } = null!;
        [PluginService] [RequiredVersion("1.0")] public static CommandManager Commands { get; private set; } = null!;
        [PluginService] [RequiredVersion("1.0")] public static ChatGui ChatGui { get; private set; } = null!;
        // @formatter:on

        private readonly string commandName = "/alic";
        private const int LinkCommandId = 1;
        private readonly DalamudLinkPayload? linkPayload;
        private readonly Configuration config;
        private bool showConfig;

        public AetheryteLinkInChatPlugin()
        {
            config = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            linkPayload = PluginInterface.AddChatLinkHandler(LinkCommandId, HandleAetheryteLink);
            ChatGui.ChatMessage += OnChatReceived;
            PluginInterface.UiBuilder.Draw += DrawConfigWindow;
            PluginInterface.UiBuilder.OpenConfigUi += OpenConfigUi;

            Commands.AddHandler(commandName, new CommandInfo(OnCommandExecute)
            {
                HelpMessage = "show configuration",
                ShowInHelp = true,
            });
        }

        public void Dispose()
        {
            PluginInterface.SavePluginConfig(config);
            PluginInterface.RemoveChatLinkHandler();
            ChatGui.ChatMessage -= OnChatReceived;
            PluginInterface.UiBuilder.Draw -= DrawConfigWindow;
            PluginInterface.UiBuilder.OpenConfigUi -= OpenConfigUi;
            Commands.RemoveHandler(commandName);
        }

        private void OnCommandExecute(string cmd, string args)
        {
            showConfig = true;
        }

        #region configUI
        private void OpenConfigUi()
        {
            showConfig = true;
        }
        private void DrawConfigWindow()
        {
            if (!showConfig) return;

            if (ImGui.Begin($"Config", ref showConfig, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Checkbox("Display the nearest aetheryte to the chat coordinates.", ref config.Enabled);
                ImGui.Indent();
                ImGui.Text("*** require Teleporter plugin ***");
                ImGui.Unindent();

                if (ImGui.Button("Save & Close"))
                {
                    showConfig = false;
                    PluginInterface.SavePluginConfig(config);
                }

                ImGui.End();
            }
        }
        #endregion

        #region chatLogic
        private void OnChatReceived(XivChatType type, uint senderId, ref SeString sender, ref SeString message,
            ref bool isHandled)
        {
            try
            {
                if (config.Enabled)
                {
                    AppendNearestAetheryteLink(ref message);
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error occurred while OnChatReceived");
            }
        }

        private void AppendNearestAetheryteLink(ref SeString message)
        {
            var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
            if (mapLink == null)
            {
                return;
            }

            // 対象のエリア内の最も近いエーテライトを探す
            var nearestAetheryte = GameData.GetExcelSheet<Aetheryte>()!
                // 対象のエリア内に限定
                .Where(x => x.Territory.Row == mapLink.TerritoryType.RowId)
                // MapMarker に変換
                .Select(x => (
                    aetheryte: x,
                    marker: GameData.GetExcelSheet<MapMarker>()!
                        // エーテライトのマーカーに限定
                        .Where(marker => marker.DataType is 3 or 4)
                        .FirstOrDefault(marker => marker.DataKey == x.RowId)
                ))
                .Where(x => x.marker != null)
                // 座標を変換
                .OrderBy(x =>
                    Math.Pow((x.marker!.X * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.XCoord, 2) +
                    Math.Pow((x.marker.Y * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.YCoord, 2))
                .FirstOrDefault();
            if (nearestAetheryte == default)
            {
                return;
            }

            // テレポ可能なエーテライト
            if (nearestAetheryte.aetheryte.IsAetheryte)
            {
                var extension = new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aetheryte),
                    linkPayload!,
                    new TextPayload($"{nearestAetheryte.aetheryte.PlaceName.Value!.Name.RawString}"),
                    RawPayload.LinkTerminator
                };
                extension.InsertRange(2, SeString.TextArrowPayloads);
                message = message.Append(extension);
            }
            // 仮設エーテライト・都市内エーテライト
            else
            {
                message = message.Append(new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aethernet),
                    new TextPayload($"{nearestAetheryte.aetheryte.AethernetName.Value!.Name.RawString}")
                });
            }
        }

        private void HandleAetheryteLink(uint id, SeString link)
        {
            if (id != LinkCommandId)
            {
                return;
            }

            Commands.ProcessCommand($"/tp {link.Payloads.OfType<TextPayload>().Last().Text}");
        }
        #endregion
    }
}
