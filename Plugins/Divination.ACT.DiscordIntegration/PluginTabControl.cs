using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable enable
namespace Divination.ACT.DiscordIntegration
{
    public partial class PluginTabControl : UserControl
    {
        private TextBox? lastFocusedTextBox;

        public PluginTabControl()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            jobIconCheckBox_CheckedChanged(sender, e);
            customStatusCheckBox_CheckedChanged(sender, e);
            authorizationTokenTextBox_TextChanged(sender, e);

            foreach (var control in GetAllControls(this))
            {
                switch (control)
                {
                    // LinkLabel をクリックしたとき, 最後にフォーカスした TextBox に挿入する
                    case LinkLabel label:
                        label.LinkClicked += (o, args) =>
                        {
                            var box = lastFocusedTextBox;
                            if (box != null && box.Enabled)
                            {
                                box.Text += label.Text;
                            }
                        };
                        break;
                    // TextBox をフォーカスしたとき, 最後にフォーカスした TextBox を記録する
                    case TextBox box when box != authorizationTokenTextBox:
                        box.Enter += (o, args) => { lastFocusedTextBox = box; };
                        break;
                }
            }
        }

        private static IEnumerable<Control> GetAllControls(Control control)
        {
            var result = new List<Control>();
            foreach (Control c in control.Controls)
            {
                result.AddRange(GetAllControls(c));
                result.Add(c);
            }

            return result;
        }

        private void jobIconCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            tooltipTextBox.Enabled = jobIconCheckBox.Checked;
        }

        private void customStatusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var value = customStatusCheckBox.Checked;
            customStatusTextBox.Enabled = value;
            jobEmojiCheckBox.Enabled = value;
            authorizationTokenTextBox.Enabled = value;
        }

        private void authorizationTokenTextBox_TextChanged(object sender, EventArgs e)
        {
            var value = !string.IsNullOrWhiteSpace(authorizationTokenTextBox.Text);
            customStatusCheckBox.Enabled = value;
            customStatusTextBox.Enabled = value;
            jobEmojiCheckBox.Enabled = value;
        }
    }
}
