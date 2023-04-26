using System;
using System.IO;

#nullable enable
namespace Divination.ACT.Companion
{
    public class Settings : PluginSettings
    {
        public Settings()
        {
            this.Bind("Voiceroid2ProxyPath", Plugin.TabControl.voiceroid2ProxyPathTextBox);
            this.Bind("Voiceroid2ProxyTTSOnLoad", Plugin.TabControl.voiceroid2ProxyTtsOnLoadCheckBox);
            this.Bind("Voiceroid2ProxyStartupMessage", Plugin.TabControl.voiceroid2ProxyStartupMessageTextBox);
            this.Bind("Voiceroid2ProxyExitOnUnload", Plugin.TabControl.voiceroid2ProxyExitOnUnloadCheckBox);

            this.Bind("ZoomHackPath", Plugin.TabControl.zoomHackPathTextBox);
            this.Bind("ZoomHackExitOnUnload", Plugin.TabControl.zoomHackExitOnUnloadCheckBox);

            this.Bind("EorzeaMarketNotePath", Plugin.TabControl.eorzeaMarketNotePathTextBox);
            this.Bind("EorzeaMarketNoteExitOnUnload", Plugin.TabControl.eorzeaMarketNotePathTextBox);

            this.Bind("FF14AnglerRepPath", Plugin.TabControl.ff14AnglerRepPathTextBox);
            this.Bind("FF14AnglerRepExitOnUnload", Plugin.TabControl.ff14AnglerRepExitOnUnloadCheckBox);
        }

        private static string Voiceroid2ProxyPath => Plugin.TabControl.voiceroid2ProxyPathTextBox.Text;
        public static bool Voiceroid2ProxyTtsOnLoad => Plugin.TabControl.voiceroid2ProxyTtsOnLoadCheckBox.Checked;

        public static string Voiceroid2ProxyStartupMessage =>
            Plugin.TabControl.voiceroid2ProxyStartupMessageTextBox.Text;

        private static bool Voiceroid2ProxyExitOnUnload =>
            Plugin.TabControl.voiceroid2ProxyExitOnUnloadCheckBox.Checked;

        private static string ZoomHackPath => Plugin.TabControl.zoomHackPathTextBox.Text;
        private static bool ZoomHackExitOnUnload => Plugin.TabControl.zoomHackExitOnUnloadCheckBox.Checked;

        private static string EorzeaMarketNotePath => Plugin.TabControl.eorzeaMarketNotePathTextBox.Text;

        private static bool EorzeaMarketNoteExitOnUnload =>
            Plugin.TabControl.eorzeaMarketNoteExitOnUnloadCheckBox.Checked;

        private static string Ff14AnglerRepPath => Plugin.TabControl.ff14AnglerRepPathTextBox.Text;
        private static bool Ff14AnglerRepExitOnUnload => Plugin.TabControl.ff14AnglerRepExitOnUnloadCheckBox.Checked;

        public static string? ExecutablePath(ProcessType type)
        {
            var path = type switch
            {
                ProcessType.Voiceroid2Proxy => Voiceroid2ProxyPath,
                ProcessType.ZoomHack => ZoomHackPath,
                ProcessType.EorzeaMarketNote => EorzeaMarketNotePath,
                ProcessType.FF14AnglerRep => Ff14AnglerRepPath,
                _ => throw new ArgumentException($"Unknown enum member: {type}")
            };

            return string.IsNullOrWhiteSpace(path) || !File.Exists(path) ? null : path;
        }

        public static bool ExitOnUnload(ProcessType type)
        {
            return type switch
            {
                ProcessType.Voiceroid2Proxy => Voiceroid2ProxyExitOnUnload,
                ProcessType.ZoomHack => ZoomHackExitOnUnload,
                ProcessType.EorzeaMarketNote => EorzeaMarketNoteExitOnUnload,
                ProcessType.FF14AnglerRep => Ff14AnglerRepExitOnUnload,
                _ => throw new ArgumentException($"Unknown enum member: {type}")
            };
        }
    }
}
