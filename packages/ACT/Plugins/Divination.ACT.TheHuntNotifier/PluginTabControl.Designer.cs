using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Divination.ACT.TheHuntNotifier
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
            Label label3;
            Label label4;
            Label label5;
            this.worldComboBox = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.rankSCheckBox = new CheckBox();
            this.rankACheckBox = new CheckBox();
            this.rankFCheckBox = new CheckBox();
            this.intervalTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // worldComboBox
            // 
            this.worldComboBox.FormattingEnabled = true;
            this.worldComboBox.Location = new Point(204, 35);
            this.worldComboBox.Name = "worldComboBox";
            this.worldComboBox.Size = new Size(172, 20);
            this.worldComboBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 38);
            label1.Name = "label1";
            label1.Size = new Size(43, 12);
            label1.TabIndex = 1;
            label1.Text = "ワールド";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(label5);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this.intervalTextBox);
            this.groupBox1.Controls.Add(this.rankFCheckBox);
            this.groupBox1.Controls.Add(this.rankACheckBox);
            this.groupBox1.Controls.Add(this.rankSCheckBox);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.worldComboBox);
            this.groupBox1.Location = new Point(32, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(446, 248);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FFXIV the Hunt";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 84);
            label2.Name = "label2";
            label2.Size = new Size(53, 12);
            label2.TabIndex = 2;
            label2.Text = "通知対象";
            // 
            // rankSCheckBox
            // 
            this.rankSCheckBox.AutoSize = true;
            this.rankSCheckBox.Checked = true;
            this.rankSCheckBox.CheckState = CheckState.Checked;
            this.rankSCheckBox.Location = new Point(54, 117);
            this.rankSCheckBox.Name = "rankSCheckBox";
            this.rankSCheckBox.Size = new Size(61, 16);
            this.rankSCheckBox.TabIndex = 3;
            this.rankSCheckBox.Text = "Rank S";
            this.rankSCheckBox.UseVisualStyleBackColor = true;
            // 
            // rankACheckBox
            // 
            this.rankACheckBox.AutoSize = true;
            this.rankACheckBox.Location = new Point(167, 117);
            this.rankACheckBox.Name = "rankACheckBox";
            this.rankACheckBox.Size = new Size(62, 16);
            this.rankACheckBox.TabIndex = 4;
            this.rankACheckBox.Text = "Rank A";
            this.rankACheckBox.UseVisualStyleBackColor = true;
            // 
            // rankFCheckBox
            // 
            this.rankFCheckBox.AutoSize = true;
            this.rankFCheckBox.Location = new Point(287, 117);
            this.rankFCheckBox.Name = "rankFCheckBox";
            this.rankFCheckBox.Size = new Size(89, 16);
            this.rankFCheckBox.TabIndex = 5;
            this.rankFCheckBox.Text = "特殊 F.A.T.E.";
            this.rankFCheckBox.UseVisualStyleBackColor = true;
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Location = new Point(204, 174);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new Size(128, 19);
            this.intervalTextBox.TabIndex = 6;
            this.intervalTextBox.Text = "30";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 177);
            label3.Name = "label3";
            label3.Size = new Size(77, 12);
            label3.TabIndex = 7;
            label3.Text = "取得間隔 (秒)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Tomato;
            label4.Location = new Point(52, 223);
            label4.Name = "label4";
            label4.Size = new Size(250, 12);
            label4.TabIndex = 8;
            label4.Text = "間隔が短すぎると IP Block される可能性があります";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(55, 204);
            label5.Name = "label5";
            label5.Size = new Size(133, 12);
            label5.TabIndex = 9;
            label5.Text = "* リロード後に反映されます";
            // 
            // PluginTabControl
            // 
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PluginTabControl";
            this.Size = new Size(537, 316);
            this.Load += new EventHandler(this.PluginTabControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public ComboBox worldComboBox;
        private GroupBox groupBox1;
        public CheckBox rankFCheckBox;
        public CheckBox rankACheckBox;
        public CheckBox rankSCheckBox;
        public TextBox intervalTextBox;
    }
}
