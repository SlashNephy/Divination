using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Divination.ACT.CustomTrigger
{
    partial class PluginTabControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.generalGroup = new GroupBox();
            this.fcChatToDiscordCheckBox = new CheckBox();
            this.copyCheckBox = new CheckBox();
            this.ttsFCChatCheckBox = new CheckBox();
            this.cwls1CheckBox = new CheckBox();
            this.ls1CheckBox = new CheckBox();
            this.gateCheckBox = new CheckBox();
            this.generalGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // generalGroup
            // 
            this.generalGroup.Controls.Add(this.gateCheckBox);
            this.generalGroup.Controls.Add(this.fcChatToDiscordCheckBox);
            this.generalGroup.Controls.Add(this.copyCheckBox);
            this.generalGroup.Controls.Add(this.ttsFCChatCheckBox);
            this.generalGroup.Controls.Add(this.cwls1CheckBox);
            this.generalGroup.Controls.Add(this.ls1CheckBox);
            this.generalGroup.Location = new Point(36, 35);
            this.generalGroup.Margin = new Padding(3, 4, 3, 4);
            this.generalGroup.Name = "generalGroup";
            this.generalGroup.Padding = new Padding(3, 4, 3, 4);
            this.generalGroup.Size = new Size(428, 216);
            this.generalGroup.TabIndex = 2;
            this.generalGroup.TabStop = false;
            this.generalGroup.Text = "Active triggers";
            // 
            // fcChatToDiscordCheckBox
            // 
            this.fcChatToDiscordCheckBox.AutoSize = true;
            this.fcChatToDiscordCheckBox.Checked = true;
            this.fcChatToDiscordCheckBox.CheckState = CheckState.Checked;
            this.fcChatToDiscordCheckBox.Location = new Point(33, 149);
            this.fcChatToDiscordCheckBox.Name = "fcChatToDiscordCheckBox";
            this.fcChatToDiscordCheckBox.Size = new Size(163, 19);
            this.fcChatToDiscordCheckBox.TabIndex = 3;
            this.fcChatToDiscordCheckBox.Text = "FC Chat to #general-vc";
            this.fcChatToDiscordCheckBox.UseVisualStyleBackColor = true;
            // 
            // copyCheckBox
            // 
            this.copyCheckBox.AutoSize = true;
            this.copyCheckBox.Checked = true;
            this.copyCheckBox.CheckState = CheckState.Checked;
            this.copyCheckBox.Location = new Point(33, 91);
            this.copyCheckBox.Name = "copyCheckBox";
            this.copyCheckBox.Size = new Size(375, 19);
            this.copyCheckBox.TabIndex = 3;
            this.copyCheckBox.Text = "Copy LS1/CWLS1 messages to clipboard when FFXIV active";
            this.copyCheckBox.UseVisualStyleBackColor = true;
            // 
            // ttsFCChatCheckBox
            // 
            this.ttsFCChatCheckBox.AutoSize = true;
            this.ttsFCChatCheckBox.Checked = true;
            this.ttsFCChatCheckBox.CheckState = CheckState.Checked;
            this.ttsFCChatCheckBox.Location = new Point(33, 120);
            this.ttsFCChatCheckBox.Name = "ttsFCChatCheckBox";
            this.ttsFCChatCheckBox.Size = new Size(236, 19);
            this.ttsFCChatCheckBox.TabIndex = 2;
            this.ttsFCChatCheckBox.Text = "TTS FC Chat when FFXIV not active";
            this.ttsFCChatCheckBox.UseVisualStyleBackColor = true;
            // 
            // cwls1CheckBox
            // 
            this.cwls1CheckBox.AutoSize = true;
            this.cwls1CheckBox.Checked = true;
            this.cwls1CheckBox.CheckState = CheckState.Checked;
            this.cwls1CheckBox.Location = new Point(33, 62);
            this.cwls1CheckBox.Name = "cwls1CheckBox";
            this.cwls1CheckBox.Size = new Size(181, 19);
            this.cwls1CheckBox.TabIndex = 1;
            this.cwls1CheckBox.Text = "CWLS1 to #world-satellite";
            this.cwls1CheckBox.UseVisualStyleBackColor = true;
            // 
            // ls1CheckBox
            // 
            this.ls1CheckBox.AutoSize = true;
            this.ls1CheckBox.Checked = true;
            this.ls1CheckBox.CheckState = CheckState.Checked;
            this.ls1CheckBox.Location = new Point(33, 33);
            this.ls1CheckBox.Name = "ls1CheckBox";
            this.ls1CheckBox.Size = new Size(161, 19);
            this.ls1CheckBox.TabIndex = 0;
            this.ls1CheckBox.Text = "LS1 to #world-satellite";
            this.ls1CheckBox.UseVisualStyleBackColor = true;
            // 
            // gateCheckBox
            // 
            this.gateCheckBox.AutoSize = true;
            this.gateCheckBox.Checked = true;
            this.gateCheckBox.CheckState = CheckState.Checked;
            this.gateCheckBox.Location = new Point(33, 178);
            this.gateCheckBox.Name = "gateCheckBox";
            this.gateCheckBox.Size = new Size(244, 19);
            this.gateCheckBox.TabIndex = 3;
            this.gateCheckBox.Text = "G.A.T.E. announce to #world-satellite";
            this.gateCheckBox.UseVisualStyleBackColor = true;
            // 
            // PluginTabControl
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.generalGroup);
            this.Font = new Font("Meiryo UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "PluginTabControl";
            this.Size = new Size(857, 678);
            this.generalGroup.ResumeLayout(false);
            this.generalGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox generalGroup;
        public CheckBox ttsFCChatCheckBox;
        public CheckBox cwls1CheckBox;
        public CheckBox ls1CheckBox;
        public CheckBox copyCheckBox;
        public CheckBox fcChatToDiscordCheckBox;
        public CheckBox gateCheckBox;
    }
}
