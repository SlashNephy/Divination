using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Divination.ACT.MobKillsCounter
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
            this.groupView = new GroupBox();
            this.labelViewY = new Label();
            this.labelViewX = new Label();
            this.udViewY = new NumericUpDown();
            this.udViewX = new NumericUpDown();
            this.labelViewOrientation = new Label();
            this.trackBarOpacity = new TrackBar();
            this.labelCurrOpacity = new Label();
            this.labelOpacity = new Label();
            this.checkViewMouseEnable = new CheckBox();
            this.checkViewVisible = new CheckBox();
            this.groupSound = new GroupBox();
            this.checkAllKillsEnable = new CheckBox();
            this.groupView.SuspendLayout();
            ((ISupportInitialize)(this.udViewY)).BeginInit();
            ((ISupportInitialize)(this.udViewX)).BeginInit();
            ((ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            this.groupSound.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupView
            // 
            this.groupView.Controls.Add(this.labelViewY);
            this.groupView.Controls.Add(this.labelViewX);
            this.groupView.Controls.Add(this.udViewY);
            this.groupView.Controls.Add(this.udViewX);
            this.groupView.Controls.Add(this.labelViewOrientation);
            this.groupView.Controls.Add(this.trackBarOpacity);
            this.groupView.Controls.Add(this.labelCurrOpacity);
            this.groupView.Controls.Add(this.labelOpacity);
            this.groupView.Controls.Add(this.checkViewMouseEnable);
            this.groupView.Controls.Add(this.checkViewVisible);
            this.groupView.Location = new Point(19, 110);
            this.groupView.Name = "groupView";
            this.groupView.Size = new Size(402, 163);
            this.groupView.TabIndex = 4;
            this.groupView.TabStop = false;
            this.groupView.Text = "ビューア";
            // 
            // labelViewY
            // 
            this.labelViewY.AutoSize = true;
            this.labelViewY.Location = new Point(222, 87);
            this.labelViewY.Name = "labelViewY";
            this.labelViewY.Size = new Size(14, 12);
            this.labelViewY.TabIndex = 11;
            this.labelViewY.Text = "Y:";
            // 
            // labelViewX
            // 
            this.labelViewX.AutoSize = true;
            this.labelViewX.Location = new Point(100, 87);
            this.labelViewX.Name = "labelViewX";
            this.labelViewX.Size = new Size(14, 12);
            this.labelViewX.TabIndex = 12;
            this.labelViewX.Text = "X:";
            // 
            // udViewY
            // 
            this.udViewY.Location = new Point(242, 85);
            this.udViewY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udViewY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.udViewY.Name = "udViewY";
            this.udViewY.Size = new Size(80, 19);
            this.udViewY.TabIndex = 9;
            this.udViewY.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udViewY.ValueChanged += new EventHandler(this.udViewY_ValueChanged);
            // 
            // udViewX
            // 
            this.udViewX.Location = new Point(120, 85);
            this.udViewX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.udViewX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.udViewX.Name = "udViewX";
            this.udViewX.Size = new Size(80, 19);
            this.udViewX.TabIndex = 10;
            this.udViewX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udViewX.ValueChanged += new EventHandler(this.udViewX_ValueChanged);
            // 
            // labelViewOrientation
            // 
            this.labelViewOrientation.AutoSize = true;
            this.labelViewOrientation.Location = new Point(16, 87);
            this.labelViewOrientation.Name = "labelViewOrientation";
            this.labelViewOrientation.Size = new Size(53, 12);
            this.labelViewOrientation.TabIndex = 8;
            this.labelViewOrientation.Text = "表示位置";
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Location = new Point(93, 116);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Minimum = 1;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new Size(234, 45);
            this.trackBarOpacity.TabIndex = 7;
            this.trackBarOpacity.TickFrequency = 10;
            this.trackBarOpacity.Value = 80;
            this.trackBarOpacity.ValueChanged += new EventHandler(this.trackBarOpacity_ValueChanged);
            // 
            // labelCurrOpacity
            // 
            this.labelCurrOpacity.AutoSize = true;
            this.labelCurrOpacity.Location = new Point(329, 121);
            this.labelCurrOpacity.Name = "labelCurrOpacity";
            this.labelCurrOpacity.Size = new Size(21, 12);
            this.labelCurrOpacity.TabIndex = 5;
            this.labelCurrOpacity.Text = "??%";
            // 
            // labelOpacity
            // 
            this.labelOpacity.AutoSize = true;
            this.labelOpacity.Location = new Point(16, 121);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new Size(53, 12);
            this.labelOpacity.TabIndex = 6;
            this.labelOpacity.Text = "不透明度";
            // 
            // checkViewMouseEnable
            // 
            this.checkViewMouseEnable.AutoSize = true;
            this.checkViewMouseEnable.Checked = true;
            this.checkViewMouseEnable.CheckState = CheckState.Checked;
            this.checkViewMouseEnable.Location = new Point(16, 54);
            this.checkViewMouseEnable.Name = "checkViewMouseEnable";
            this.checkViewMouseEnable.Size = new Size(157, 16);
            this.checkViewMouseEnable.TabIndex = 3;
            this.checkViewMouseEnable.Text = "マウスで移動できるようにする";
            this.checkViewMouseEnable.UseVisualStyleBackColor = true;
            this.checkViewMouseEnable.CheckedChanged += new EventHandler(this.checkViewMouseEnable_CheckedChanged);
            // 
            // checkViewVisible
            // 
            this.checkViewVisible.AutoSize = true;
            this.checkViewVisible.Checked = true;
            this.checkViewVisible.CheckState = CheckState.Checked;
            this.checkViewVisible.Location = new Point(16, 27);
            this.checkViewVisible.Name = "checkViewVisible";
            this.checkViewVisible.Size = new Size(67, 16);
            this.checkViewVisible.TabIndex = 2;
            this.checkViewVisible.Text = "表示する";
            this.checkViewVisible.UseVisualStyleBackColor = true;
            this.checkViewVisible.CheckedChanged += new EventHandler(this.checkViewVisible_CheckedChanged);
            // 
            // groupSound
            // 
            this.groupSound.Controls.Add(this.checkAllKillsEnable);
            this.groupSound.Location = new Point(19, 24);
            this.groupSound.Name = "groupSound";
            this.groupSound.Size = new Size(402, 66);
            this.groupSound.TabIndex = 5;
            this.groupSound.TabStop = false;
            this.groupSound.Text = "カウント";
            // 
            // checkAllKillsEnable
            // 
            this.checkAllKillsEnable.AutoSize = true;
            this.checkAllKillsEnable.Location = new Point(16, 27);
            this.checkAllKillsEnable.Name = "checkAllKillsEnable";
            this.checkAllKillsEnable.Size = new Size(187, 16);
            this.checkAllKillsEnable.TabIndex = 1;
            this.checkAllKillsEnable.Text = "すべてのPCの討伐数をカウントする";
            this.checkAllKillsEnable.UseVisualStyleBackColor = true;
            // 
            // PluginTabControl
            // 
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupSound);
            this.Controls.Add(this.groupView);
            this.Name = "PluginTabControl";
            this.Size = new Size(451, 329);
            this.Load += new EventHandler(this.PluginTabControl_Load);
            this.groupView.ResumeLayout(false);
            this.groupView.PerformLayout();
            ((ISupportInitialize)(this.udViewY)).EndInit();
            ((ISupportInitialize)(this.udViewX)).EndInit();
            ((ISupportInitialize)(this.trackBarOpacity)).EndInit();
            this.groupSound.ResumeLayout(false);
            this.groupSound.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private GroupBox groupView;
        private Label labelViewY;
        private Label labelViewX;
        private Label labelViewOrientation;
        private Label labelCurrOpacity;
        private Label labelOpacity;
        private GroupBox groupSound;
        public NumericUpDown udViewY;
        public NumericUpDown udViewX;
        public CheckBox checkViewMouseEnable;
        public CheckBox checkViewVisible;
        public CheckBox checkAllKillsEnable;
        public TrackBar trackBarOpacity;
    }
}
