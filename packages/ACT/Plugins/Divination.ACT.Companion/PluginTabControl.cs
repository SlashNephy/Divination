using System;
using System.Diagnostics;
using System.Windows.Forms;

#nullable enable
namespace Divination.ACT.Companion
{
    public partial class PluginTabControl : UserControl
    {
        public PluginTabControl()
        {
            InitializeComponent();
        }

        private void ttsOnLoadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            voiceroid2ProxyStartupMessageTextBox.Enabled = voiceroid2ProxyTtsOnLoadCheckBox.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Voiceroid2Proxy.exe への参照",
                FileName = "Divination.Voiceroid2Proxy.exe",
                InitialDirectory = Plugin.AssemblyDirectory,
                Filter = "実行ファイル(*.exe)|*.exe"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                voiceroid2ProxyPathTextBox.Text = dialog.FileName;
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/anoyetta/FFXIV-Zoom-Hack");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "FFXIVZoomHack.exe への参照",
                FileName = "FFXIVZoomHack.exe",
                InitialDirectory = Plugin.AssemblyDirectory,
                Filter = "実行ファイル(*.exe)|*.exe"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                zoomHackPathTextBox.Text = dialog.FileName;
            }
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            Process.Start("https://ff14marketnote.ownway.info/ja/app/windows");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "EorzeaMarketNote.exe への参照",
                FileName = "EorzeaMarketNote.exe",
                InitialDirectory = Plugin.AssemblyDirectory,
                Filter = "実行ファイル(*.exe)|*.exe"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                eorzeaMarketNotePathTextBox.Text = dialog.FileName;
            }
        }

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            Process.Start("https://jp.ff14angler.com/rep/download/");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "FF14AnglerRep.exe への参照",
                FileName = "FF14AnglerRep.exe",
                InitialDirectory = Plugin.AssemblyDirectory,
                Filter = "実行ファイル(*.exe)|*.exe"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ff14AnglerRepPathTextBox.Text = dialog.FileName;
            }
        }
    }
}
