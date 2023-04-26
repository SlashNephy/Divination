using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Divination.ACT.Companion
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
            Label label1;
            Label label2;
            this.generalGroup = new GroupBox();
            this.button1 = new Button();
            this.voiceroid2ProxyPathTextBox = new TextBox();
            this.voiceroid2ProxyExitOnUnloadCheckBox = new CheckBox();
            this.voiceroid2ProxyStartupMessageTextBox = new TextBox();
            this.voiceroid2ProxyTtsOnLoadCheckBox = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.linkLabel1 = new LinkLabel();
            this.label3 = new Label();
            this.zoomHackExitOnUnloadCheckBox = new CheckBox();
            this.button2 = new Button();
            this.zoomHackPathTextBox = new TextBox();
            this.groupBox2 = new GroupBox();
            this.linkLabel2 = new LinkLabel();
            this.label4 = new Label();
            this.eorzeaMarketNoteExitOnUnloadCheckBox = new CheckBox();
            this.button3 = new Button();
            this.eorzeaMarketNotePathTextBox = new TextBox();
            this.groupBox3 = new GroupBox();
            this.linkLabel3 = new LinkLabel();
            this.label5 = new Label();
            this.ff14AnglerRepExitOnUnloadCheckBox = new CheckBox();
            this.button4 = new Button();
            this.ff14AnglerRepPathTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            this.generalGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(68, 111);
            label1.Name = "label1";
            label1.Size = new Size(373, 30);
            label1.TabIndex = 1;
            label1.Text = "これを有効にすることで, 非同期に VOICEROID2 の初期化を行うことができ,\r\n初回の読み上げに時間がかかる問題を解消します。";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(45, 156);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 3;
            label2.Text = "読み上げるメッセージ";
            // 
            // generalGroup
            // 
            this.generalGroup.Controls.Add(this.button1);
            this.generalGroup.Controls.Add(this.voiceroid2ProxyPathTextBox);
            this.generalGroup.Controls.Add(this.voiceroid2ProxyExitOnUnloadCheckBox);
            this.generalGroup.Controls.Add(label2);
            this.generalGroup.Controls.Add(this.voiceroid2ProxyStartupMessageTextBox);
            this.generalGroup.Controls.Add(label1);
            this.generalGroup.Controls.Add(this.voiceroid2ProxyTtsOnLoadCheckBox);
            this.generalGroup.Location = new Point(36, 35);
            this.generalGroup.Margin = new Padding(3, 4, 3, 4);
            this.generalGroup.Name = "generalGroup";
            this.generalGroup.Padding = new Padding(3, 4, 3, 4);
            this.generalGroup.Size = new Size(507, 311);
            this.generalGroup.TabIndex = 2;
            this.generalGroup.TabStop = false;
            this.generalGroup.Text = "Divination.Voiceroid2Proxy.exe";
            // 
            // button1
            // 
            this.button1.Location = new Point(409, 36);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            // 
            // voiceroid2ProxyPathTextBox
            // 
            this.voiceroid2ProxyPathTextBox.Location = new Point(27, 36);
            this.voiceroid2ProxyPathTextBox.Name = "voiceroid2ProxyPathTextBox";
            this.voiceroid2ProxyPathTextBox.Size = new Size(376, 23);
            this.voiceroid2ProxyPathTextBox.TabIndex = 4;
            // 
            // voiceroid2ProxyExitOnUnloadCheckBox
            // 
            this.voiceroid2ProxyExitOnUnloadCheckBox.AutoSize = true;
            this.voiceroid2ProxyExitOnUnloadCheckBox.Location = new Point(27, 266);
            this.voiceroid2ProxyExitOnUnloadCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.voiceroid2ProxyExitOnUnloadCheckBox.Name = "voiceroid2ProxyExitOnUnloadCheckBox";
            this.voiceroid2ProxyExitOnUnloadCheckBox.Size = new Size(204, 19);
            this.voiceroid2ProxyExitOnUnloadCheckBox.TabIndex = 3;
            this.voiceroid2ProxyExitOnUnloadCheckBox.Text = "プラグイン停止時にプロセスを終了する";
            this.voiceroid2ProxyExitOnUnloadCheckBox.UseVisualStyleBackColor = false;
            // 
            // voiceroid2ProxyStartupMessageTextBox
            // 
            this.voiceroid2ProxyStartupMessageTextBox.Location = new Point(71, 184);
            this.voiceroid2ProxyStartupMessageTextBox.Margin = new Padding(3, 4, 3, 4);
            this.voiceroid2ProxyStartupMessageTextBox.Multiline = true;
            this.voiceroid2ProxyStartupMessageTextBox.Name = "voiceroid2ProxyStartupMessageTextBox";
            this.voiceroid2ProxyStartupMessageTextBox.Size = new Size(383, 58);
            this.voiceroid2ProxyStartupMessageTextBox.TabIndex = 2;
            this.voiceroid2ProxyStartupMessageTextBox.Text = "準備完了！";
            // 
            // voiceroid2ProxyTtsOnLoadCheckBox
            // 
            this.voiceroid2ProxyTtsOnLoadCheckBox.AutoSize = true;
            this.voiceroid2ProxyTtsOnLoadCheckBox.Checked = true;
            this.voiceroid2ProxyTtsOnLoadCheckBox.CheckState = CheckState.Checked;
            this.voiceroid2ProxyTtsOnLoadCheckBox.Location = new Point(27, 87);
            this.voiceroid2ProxyTtsOnLoadCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.voiceroid2ProxyTtsOnLoadCheckBox.Name = "voiceroid2ProxyTtsOnLoadCheckBox";
            this.voiceroid2ProxyTtsOnLoadCheckBox.Size = new Size(223, 19);
            this.voiceroid2ProxyTtsOnLoadCheckBox.TabIndex = 0;
            this.voiceroid2ProxyTtsOnLoadCheckBox.Text = "プラグイン開始時にメッセージを読み上げる";
            this.voiceroid2ProxyTtsOnLoadCheckBox.UseVisualStyleBackColor = true;
            this.voiceroid2ProxyTtsOnLoadCheckBox.CheckedChanged += new EventHandler(this.ttsOnLoadCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.zoomHackExitOnUnloadCheckBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.zoomHackPathTextBox);
            this.groupBox1.Location = new Point(36, 365);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(507, 138);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FFXIVZoomHack.exe";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new Point(411, 29);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(43, 15);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "配布元";
            this.linkLabel1.Click += new EventHandler(this.linkLabel1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(24, 29);
            this.label3.Name = "label3";
            this.label3.Size = new Size(299, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "カメラのズームの最大距離や FOV を上書きできるツールです。";
            // 
            // zoomHackExitOnUnloadCheckBox
            // 
            this.zoomHackExitOnUnloadCheckBox.AutoSize = true;
            this.zoomHackExitOnUnloadCheckBox.Checked = true;
            this.zoomHackExitOnUnloadCheckBox.CheckState = CheckState.Checked;
            this.zoomHackExitOnUnloadCheckBox.Location = new Point(27, 100);
            this.zoomHackExitOnUnloadCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.zoomHackExitOnUnloadCheckBox.Name = "zoomHackExitOnUnloadCheckBox";
            this.zoomHackExitOnUnloadCheckBox.Size = new Size(204, 19);
            this.zoomHackExitOnUnloadCheckBox.TabIndex = 7;
            this.zoomHackExitOnUnloadCheckBox.Text = "プラグイン停止時にプロセスを終了する";
            this.zoomHackExitOnUnloadCheckBox.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Location = new Point(409, 60);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "参照";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            // 
            // zoomHackPathTextBox
            // 
            this.zoomHackPathTextBox.Location = new Point(27, 60);
            this.zoomHackPathTextBox.Name = "zoomHackPathTextBox";
            this.zoomHackPathTextBox.Size = new Size(376, 23);
            this.zoomHackPathTextBox.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.linkLabel2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.eorzeaMarketNoteExitOnUnloadCheckBox);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.eorzeaMarketNotePathTextBox);
            this.groupBox2.Location = new Point(36, 522);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(507, 138);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "EorzeaMarketNote.exe";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new Point(411, 29);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new Size(43, 15);
            this.linkLabel2.TabIndex = 9;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "配布元";
            this.linkLabel2.Click += new EventHandler(this.linkLabel2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(24, 29);
            this.label4.Name = "label4";
            this.label4.Size = new Size(239, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "アイテムのマーケット価格を調査できるツールです。";
            // 
            // eorzeaMarketNoteExitOnUnloadCheckBox
            // 
            this.eorzeaMarketNoteExitOnUnloadCheckBox.AutoSize = true;
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Checked = true;
            this.eorzeaMarketNoteExitOnUnloadCheckBox.CheckState = CheckState.Checked;
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Location = new Point(27, 100);
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Name = "eorzeaMarketNoteExitOnUnloadCheckBox";
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Size = new Size(204, 19);
            this.eorzeaMarketNoteExitOnUnloadCheckBox.TabIndex = 7;
            this.eorzeaMarketNoteExitOnUnloadCheckBox.Text = "プラグイン停止時にプロセスを終了する";
            this.eorzeaMarketNoteExitOnUnloadCheckBox.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.Location = new Point(409, 60);
            this.button3.Name = "button3";
            this.button3.Size = new Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "参照";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            // 
            // eorzeaMarketNotePathTextBox
            // 
            this.eorzeaMarketNotePathTextBox.Location = new Point(27, 60);
            this.eorzeaMarketNotePathTextBox.Name = "eorzeaMarketNotePathTextBox";
            this.eorzeaMarketNotePathTextBox.Size = new Size(376, 23);
            this.eorzeaMarketNotePathTextBox.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.linkLabel3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ff14AnglerRepExitOnUnloadCheckBox);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.ff14AnglerRepPathTextBox);
            this.groupBox3.Location = new Point(560, 365);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(507, 138);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FF14AnglerRep.exe";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new Point(411, 29);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new Size(43, 15);
            this.linkLabel3.TabIndex = 9;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "配布元";
            this.linkLabel3.Click += new EventHandler(this.linkLabel3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(24, 29);
            this.label5.Name = "label5";
            this.label5.Size = new Size(313, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "釣り上げた魚や分解した結果の統計情報を送信するツールです。";
            // 
            // ff14AnglerRepExitOnUnloadCheckBox
            // 
            this.ff14AnglerRepExitOnUnloadCheckBox.AutoSize = true;
            this.ff14AnglerRepExitOnUnloadCheckBox.Checked = true;
            this.ff14AnglerRepExitOnUnloadCheckBox.CheckState = CheckState.Checked;
            this.ff14AnglerRepExitOnUnloadCheckBox.Location = new Point(27, 100);
            this.ff14AnglerRepExitOnUnloadCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.ff14AnglerRepExitOnUnloadCheckBox.Name = "ff14AnglerRepExitOnUnloadCheckBox";
            this.ff14AnglerRepExitOnUnloadCheckBox.Size = new Size(204, 19);
            this.ff14AnglerRepExitOnUnloadCheckBox.TabIndex = 7;
            this.ff14AnglerRepExitOnUnloadCheckBox.Text = "プラグイン停止時にプロセスを終了する";
            this.ff14AnglerRepExitOnUnloadCheckBox.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.Location = new Point(409, 60);
            this.button4.Name = "button4";
            this.button4.Size = new Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "参照";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click);
            // 
            // ff14AnglerRepPathTextBox
            // 
            this.ff14AnglerRepPathTextBox.Location = new Point(27, 60);
            this.ff14AnglerRepPathTextBox.Name = "ff14AnglerRepPathTextBox";
            this.ff14AnglerRepPathTextBox.Size = new Size(376, 23);
            this.ff14AnglerRepPathTextBox.TabIndex = 6;
            // 
            // PluginTabControl
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.generalGroup);
            this.Font = new Font("Meiryo UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "PluginTabControl";
            this.Size = new Size(1219, 678);
            this.generalGroup.ResumeLayout(false);
            this.generalGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox generalGroup;
        public CheckBox voiceroid2ProxyTtsOnLoadCheckBox;
        public CheckBox voiceroid2ProxyExitOnUnloadCheckBox;
        public TextBox voiceroid2ProxyStartupMessageTextBox;
        private Button button1;
        public TextBox voiceroid2ProxyPathTextBox;
        private GroupBox groupBox1;
        private LinkLabel linkLabel1;
        private Label label3;
        public CheckBox zoomHackExitOnUnloadCheckBox;
        private Button button2;
        public TextBox zoomHackPathTextBox;
        private GroupBox groupBox2;
        private LinkLabel linkLabel2;
        private Label label4;
        public CheckBox eorzeaMarketNoteExitOnUnloadCheckBox;
        private Button button3;
        public TextBox eorzeaMarketNotePathTextBox;
        private GroupBox groupBox3;
        private LinkLabel linkLabel3;
        private Label label5;
        public CheckBox ff14AnglerRepExitOnUnloadCheckBox;
        private Button button4;
        public TextBox ff14AnglerRepPathTextBox;
    }
}
