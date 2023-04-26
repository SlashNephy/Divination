using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Divination.ACT.DiscordIntegration
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
            this.components = new Container();
            Label label1;
            Label label2;
            Label label3;
            GroupBox variablesGroupBox;
            LinkLabel linkLabel13;
            LinkLabel linkLabel9;
            LinkLabel linkLabel25;
            LinkLabel linkLabel23;
            LinkLabel linkLabel24;
            LinkLabel linkLabel21;
            LinkLabel linkLabel22;
            LinkLabel linkLabel20;
            LinkLabel linkLabel19;
            LinkLabel linkLabel18;
            LinkLabel linkLabel17;
            LinkLabel linkLabel16;
            LinkLabel linkLabel12;
            LinkLabel linkLabel11;
            LinkLabel linkLabel10;
            LinkLabel linkLabel8;
            LinkLabel linkLabel7;
            Label label5;
            LinkLabel linkLabel6;
            LinkLabel linkLabel5;
            LinkLabel linkLabel4;
            LinkLabel linkLabel3;
            LinkLabel linkLabel2;
            LinkLabel linkLabel1;
            Label label11;
            Label label12;
            Label label13;
            Label label14;
            ToolTip variableToolTip;
            GroupBox formatGroup;
            GroupBox groupBox1;
            Label label9;
            Label label8;
            Label label4;
            GroupBox groupBox2;
            Label label7;
            Label label6;
            this.zoneSampleLabel = new Label();
            this.dutySampleLabel = new Label();
            this.targetSampleLabel = new Label();
            this.thppSampleLabel = new Label();
            this.thpSampleLabel = new Label();
            this.mppSampleLabel = new Label();
            this.mpSampleLabel = new Label();
            this.hppSampleLabel = new Label();
            this.hpSampleLabel = new Label();
            this.zSampleLabel = new Label();
            this.ySampleLabel = new Label();
            this.xSampleLabel = new Label();
            this.partySampleLabel = new Label();
            this.homeWorldSampleLabel = new Label();
            this.worldSampleLabel = new Label();
            this.placeSampleLabel = new Label();
            this.regionSampleLabel = new Label();
            this.jobNameSampleLabel = new Label();
            this.levelSampleLabel = new Label();
            this.jobSampleLabel = new Label();
            this.characterSampleLabel = new Label();
            this.actZoneSampleLabel = new Label();
            this.statusSampleLabel = new Label();
            this.requireTargetingOnCombatCheckBox = new CheckBox();
            this.detailsInCombatTextBox = new TextBox();
            this.resetTimerCheckBox = new CheckBox();
            this.logoTooltipTextBox = new TextBox();
            this.jobIconCheckBox = new CheckBox();
            this.tooltipTextBox = new TextBox();
            this.stateTextBox = new TextBox();
            this.detailsTextBox = new TextBox();
            this.onlineStatusEmojiCheckBox = new CheckBox();
            this.customStatusOnContentsTextBox = new TextBox();
            this.emojiIdTextBox = new TextBox();
            this.customStatusCheckBox = new CheckBox();
            this.customStatusTextDefaultTextBox = new TextBox();
            this.customStatusTextBox = new TextBox();
            this.jobEmojiCheckBox = new CheckBox();
            this.authorizationTokenTextBox = new TextBox();
            this.sharlayanLabel = new Label();
            this.processLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            variablesGroupBox = new GroupBox();
            linkLabel13 = new LinkLabel();
            linkLabel9 = new LinkLabel();
            linkLabel25 = new LinkLabel();
            linkLabel23 = new LinkLabel();
            linkLabel24 = new LinkLabel();
            linkLabel21 = new LinkLabel();
            linkLabel22 = new LinkLabel();
            linkLabel20 = new LinkLabel();
            linkLabel19 = new LinkLabel();
            linkLabel18 = new LinkLabel();
            linkLabel17 = new LinkLabel();
            linkLabel16 = new LinkLabel();
            linkLabel12 = new LinkLabel();
            linkLabel11 = new LinkLabel();
            linkLabel10 = new LinkLabel();
            linkLabel8 = new LinkLabel();
            linkLabel7 = new LinkLabel();
            label5 = new Label();
            linkLabel6 = new LinkLabel();
            linkLabel5 = new LinkLabel();
            linkLabel4 = new LinkLabel();
            linkLabel3 = new LinkLabel();
            linkLabel2 = new LinkLabel();
            linkLabel1 = new LinkLabel();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            variableToolTip = new ToolTip(this.components);
            formatGroup = new GroupBox();
            groupBox1 = new GroupBox();
            label9 = new Label();
            label8 = new Label();
            label4 = new Label();
            groupBox2 = new GroupBox();
            label7 = new Label();
            label6 = new Label();
            variablesGroupBox.SuspendLayout();
            formatGroup.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 42);
            label1.Name = "label1";
            label1.Size = new Size(109, 15);
            label1.TabIndex = 3;
            label1.Text = "Details (非戦闘時)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 104);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "State";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(71, 205);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 6;
            label3.Text = "Tooltip";
            // 
            // variablesGroupBox
            // 
            variablesGroupBox.Controls.Add(this.zoneSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel13);
            variablesGroupBox.Controls.Add(this.dutySampleLabel);
            variablesGroupBox.Controls.Add(linkLabel9);
            variablesGroupBox.Controls.Add(this.targetSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel25);
            variablesGroupBox.Controls.Add(this.thppSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel23);
            variablesGroupBox.Controls.Add(this.thpSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel24);
            variablesGroupBox.Controls.Add(this.mppSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel21);
            variablesGroupBox.Controls.Add(this.mpSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel22);
            variablesGroupBox.Controls.Add(this.hppSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel20);
            variablesGroupBox.Controls.Add(this.hpSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel19);
            variablesGroupBox.Controls.Add(this.zSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel18);
            variablesGroupBox.Controls.Add(this.ySampleLabel);
            variablesGroupBox.Controls.Add(linkLabel17);
            variablesGroupBox.Controls.Add(this.xSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel16);
            variablesGroupBox.Controls.Add(this.partySampleLabel);
            variablesGroupBox.Controls.Add(linkLabel12);
            variablesGroupBox.Controls.Add(this.homeWorldSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel11);
            variablesGroupBox.Controls.Add(this.worldSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel10);
            variablesGroupBox.Controls.Add(this.placeSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel8);
            variablesGroupBox.Controls.Add(this.regionSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel7);
            variablesGroupBox.Controls.Add(label5);
            variablesGroupBox.Controls.Add(this.jobNameSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel6);
            variablesGroupBox.Controls.Add(this.levelSampleLabel);
            variablesGroupBox.Controls.Add(this.jobSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel5);
            variablesGroupBox.Controls.Add(linkLabel4);
            variablesGroupBox.Controls.Add(this.characterSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel3);
            variablesGroupBox.Controls.Add(this.actZoneSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel2);
            variablesGroupBox.Controls.Add(this.statusSampleLabel);
            variablesGroupBox.Controls.Add(linkLabel1);
            variablesGroupBox.Location = new Point(598, 18);
            variablesGroupBox.Margin = new Padding(3, 4, 3, 4);
            variablesGroupBox.Name = "variablesGroupBox";
            variablesGroupBox.Padding = new Padding(3, 4, 3, 4);
            variablesGroupBox.Size = new Size(445, 751);
            variablesGroupBox.TabIndex = 3;
            variablesGroupBox.TabStop = false;
            variablesGroupBox.Text = "利用可能な変数";
            // 
            // zoneSampleLabel
            // 
            this.zoneSampleLabel.AutoSize = true;
            this.zoneSampleLabel.Location = new Point(198, 158);
            this.zoneSampleLabel.Name = "zoneSampleLabel";
            this.zoneSampleLabel.Size = new Size(19, 15);
            this.zoneSampleLabel.TabIndex = 54;
            this.zoneSampleLabel.Text = "...";
            // 
            // linkLabel13
            // 
            linkLabel13.AutoSize = true;
            linkLabel13.Location = new Point(35, 158);
            linkLabel13.Name = "linkLabel13";
            linkLabel13.Size = new Size(48, 15);
            linkLabel13.TabIndex = 53;
            linkLabel13.TabStop = true;
            linkLabel13.Text = "{zone}";
            variableToolTip.SetToolTip(linkLabel13, "ターゲットの現在の HP %");
            // 
            // dutySampleLabel
            // 
            this.dutySampleLabel.AutoSize = true;
            this.dutySampleLabel.Location = new Point(198, 182);
            this.dutySampleLabel.Name = "dutySampleLabel";
            this.dutySampleLabel.Size = new Size(19, 15);
            this.dutySampleLabel.TabIndex = 52;
            this.dutySampleLabel.Text = "...";
            // 
            // linkLabel9
            // 
            linkLabel9.AutoSize = true;
            linkLabel9.Location = new Point(35, 182);
            linkLabel9.Name = "linkLabel9";
            linkLabel9.Size = new Size(47, 15);
            linkLabel9.TabIndex = 51;
            linkLabel9.TabStop = true;
            linkLabel9.Text = "{duty}";
            variableToolTip.SetToolTip(linkLabel9, "キャラクターの現在いるコンテンツ名");
            // 
            // targetSampleLabel
            // 
            this.targetSampleLabel.AutoSize = true;
            this.targetSampleLabel.Location = new Point(198, 542);
            this.targetSampleLabel.Name = "targetSampleLabel";
            this.targetSampleLabel.Size = new Size(19, 15);
            this.targetSampleLabel.TabIndex = 50;
            this.targetSampleLabel.Text = "...";
            // 
            // linkLabel25
            // 
            linkLabel25.AutoSize = true;
            linkLabel25.Location = new Point(35, 542);
            linkLabel25.Name = "linkLabel25";
            linkLabel25.Size = new Size(57, 15);
            linkLabel25.TabIndex = 49;
            linkLabel25.TabStop = true;
            linkLabel25.Text = "{target}";
            variableToolTip.SetToolTip(linkLabel25, "ターゲットの名前");
            // 
            // thppSampleLabel
            // 
            this.thppSampleLabel.AutoSize = true;
            this.thppSampleLabel.Location = new Point(198, 590);
            this.thppSampleLabel.Name = "thppSampleLabel";
            this.thppSampleLabel.Size = new Size(19, 15);
            this.thppSampleLabel.TabIndex = 48;
            this.thppSampleLabel.Text = "...";
            // 
            // linkLabel23
            // 
            linkLabel23.AutoSize = true;
            linkLabel23.Location = new Point(35, 590);
            linkLabel23.Name = "linkLabel23";
            linkLabel23.Size = new Size(47, 15);
            linkLabel23.TabIndex = 47;
            linkLabel23.TabStop = true;
            linkLabel23.Text = "{thpp}";
            variableToolTip.SetToolTip(linkLabel23, "ターゲットの現在の HP %");
            // 
            // thpSampleLabel
            // 
            this.thpSampleLabel.AutoSize = true;
            this.thpSampleLabel.Location = new Point(198, 566);
            this.thpSampleLabel.Name = "thpSampleLabel";
            this.thpSampleLabel.Size = new Size(19, 15);
            this.thpSampleLabel.TabIndex = 46;
            this.thpSampleLabel.Text = "...";
            // 
            // linkLabel24
            // 
            linkLabel24.AutoSize = true;
            linkLabel24.Location = new Point(35, 566);
            linkLabel24.Name = "linkLabel24";
            linkLabel24.Size = new Size(40, 15);
            linkLabel24.TabIndex = 45;
            linkLabel24.TabStop = true;
            linkLabel24.Text = "{thp}";
            variableToolTip.SetToolTip(linkLabel24, "ターゲットの現在の HP");
            // 
            // mppSampleLabel
            // 
            this.mppSampleLabel.AutoSize = true;
            this.mppSampleLabel.Location = new Point(198, 518);
            this.mppSampleLabel.Name = "mppSampleLabel";
            this.mppSampleLabel.Size = new Size(19, 15);
            this.mppSampleLabel.TabIndex = 44;
            this.mppSampleLabel.Text = "...";
            // 
            // linkLabel21
            // 
            linkLabel21.AutoSize = true;
            linkLabel21.Location = new Point(35, 518);
            linkLabel21.Name = "linkLabel21";
            linkLabel21.Size = new Size(47, 15);
            linkLabel21.TabIndex = 43;
            linkLabel21.TabStop = true;
            linkLabel21.Text = "{mpp}";
            variableToolTip.SetToolTip(linkLabel21, "キャラクターの現在の MP %");
            // 
            // mpSampleLabel
            // 
            this.mpSampleLabel.AutoSize = true;
            this.mpSampleLabel.Location = new Point(198, 494);
            this.mpSampleLabel.Name = "mpSampleLabel";
            this.mpSampleLabel.Size = new Size(19, 15);
            this.mpSampleLabel.TabIndex = 42;
            this.mpSampleLabel.Text = "...";
            // 
            // linkLabel22
            // 
            linkLabel22.AutoSize = true;
            linkLabel22.Location = new Point(35, 494);
            linkLabel22.Name = "linkLabel22";
            linkLabel22.Size = new Size(40, 15);
            linkLabel22.TabIndex = 41;
            linkLabel22.TabStop = true;
            linkLabel22.Text = "{mp}";
            variableToolTip.SetToolTip(linkLabel22, "キャラクターの現在の MP");
            // 
            // hppSampleLabel
            // 
            this.hppSampleLabel.AutoSize = true;
            this.hppSampleLabel.Location = new Point(198, 470);
            this.hppSampleLabel.Name = "hppSampleLabel";
            this.hppSampleLabel.Size = new Size(19, 15);
            this.hppSampleLabel.TabIndex = 40;
            this.hppSampleLabel.Text = "...";
            // 
            // linkLabel20
            // 
            linkLabel20.AutoSize = true;
            linkLabel20.Location = new Point(35, 470);
            linkLabel20.Name = "linkLabel20";
            linkLabel20.Size = new Size(42, 15);
            linkLabel20.TabIndex = 39;
            linkLabel20.TabStop = true;
            linkLabel20.Text = "{hpp}";
            variableToolTip.SetToolTip(linkLabel20, "キャラクターの現在の HP %");
            // 
            // hpSampleLabel
            // 
            this.hpSampleLabel.AutoSize = true;
            this.hpSampleLabel.Location = new Point(198, 446);
            this.hpSampleLabel.Name = "hpSampleLabel";
            this.hpSampleLabel.Size = new Size(19, 15);
            this.hpSampleLabel.TabIndex = 38;
            this.hpSampleLabel.Text = "...";
            // 
            // linkLabel19
            // 
            linkLabel19.AutoSize = true;
            linkLabel19.Location = new Point(35, 446);
            linkLabel19.Name = "linkLabel19";
            linkLabel19.Size = new Size(35, 15);
            linkLabel19.TabIndex = 37;
            linkLabel19.TabStop = true;
            linkLabel19.Text = "{hp}";
            variableToolTip.SetToolTip(linkLabel19, "キャラクターの現在の HP");
            // 
            // zSampleLabel
            // 
            this.zSampleLabel.AutoSize = true;
            this.zSampleLabel.Location = new Point(198, 422);
            this.zSampleLabel.Name = "zSampleLabel";
            this.zSampleLabel.Size = new Size(19, 15);
            this.zSampleLabel.TabIndex = 36;
            this.zSampleLabel.Text = "...";
            // 
            // linkLabel18
            // 
            linkLabel18.AutoSize = true;
            linkLabel18.Location = new Point(35, 422);
            linkLabel18.Name = "linkLabel18";
            linkLabel18.Size = new Size(27, 15);
            linkLabel18.TabIndex = 35;
            linkLabel18.TabStop = true;
            linkLabel18.Text = "{z}";
            variableToolTip.SetToolTip(linkLabel18, "キャラクターの現在の Z 座標");
            // 
            // ySampleLabel
            // 
            this.ySampleLabel.AutoSize = true;
            this.ySampleLabel.Location = new Point(198, 398);
            this.ySampleLabel.Name = "ySampleLabel";
            this.ySampleLabel.Size = new Size(19, 15);
            this.ySampleLabel.TabIndex = 34;
            this.ySampleLabel.Text = "...";
            // 
            // linkLabel17
            // 
            linkLabel17.AutoSize = true;
            linkLabel17.Location = new Point(35, 398);
            linkLabel17.Name = "linkLabel17";
            linkLabel17.Size = new Size(28, 15);
            linkLabel17.TabIndex = 33;
            linkLabel17.TabStop = true;
            linkLabel17.Text = "{y}";
            variableToolTip.SetToolTip(linkLabel17, "キャラクターの現在の Y 座標");
            // 
            // xSampleLabel
            // 
            this.xSampleLabel.AutoSize = true;
            this.xSampleLabel.Location = new Point(198, 374);
            this.xSampleLabel.Name = "xSampleLabel";
            this.xSampleLabel.Size = new Size(19, 15);
            this.xSampleLabel.TabIndex = 32;
            this.xSampleLabel.Text = "...";
            // 
            // linkLabel16
            // 
            linkLabel16.AutoSize = true;
            linkLabel16.Location = new Point(35, 374);
            linkLabel16.Name = "linkLabel16";
            linkLabel16.Size = new Size(28, 15);
            linkLabel16.TabIndex = 31;
            linkLabel16.TabStop = true;
            linkLabel16.Text = "{x}";
            variableToolTip.SetToolTip(linkLabel16, "キャラクターの現在の X 座標");
            // 
            // partySampleLabel
            // 
            this.partySampleLabel.AutoSize = true;
            this.partySampleLabel.Location = new Point(198, 350);
            this.partySampleLabel.Name = "partySampleLabel";
            this.partySampleLabel.Size = new Size(19, 15);
            this.partySampleLabel.TabIndex = 24;
            this.partySampleLabel.Text = "...";
            // 
            // linkLabel12
            // 
            linkLabel12.AutoSize = true;
            linkLabel12.Location = new Point(35, 350);
            linkLabel12.Name = "linkLabel12";
            linkLabel12.Size = new Size(52, 15);
            linkLabel12.TabIndex = 23;
            linkLabel12.TabStop = true;
            linkLabel12.Text = "{party}";
            variableToolTip.SetToolTip(linkLabel12, "キャラクターのパーティ状況");
            // 
            // homeWorldSampleLabel
            // 
            this.homeWorldSampleLabel.AutoSize = true;
            this.homeWorldSampleLabel.Location = new Point(198, 326);
            this.homeWorldSampleLabel.Name = "homeWorldSampleLabel";
            this.homeWorldSampleLabel.Size = new Size(19, 15);
            this.homeWorldSampleLabel.TabIndex = 22;
            this.homeWorldSampleLabel.Text = "...";
            // 
            // linkLabel11
            // 
            linkLabel11.AutoSize = true;
            linkLabel11.Location = new Point(35, 326);
            linkLabel11.Name = "linkLabel11";
            linkLabel11.Size = new Size(93, 15);
            linkLabel11.TabIndex = 21;
            linkLabel11.TabStop = true;
            linkLabel11.Text = "{home_world}";
            variableToolTip.SetToolTip(linkLabel11, "キャラクターのホームワールド名");
            // 
            // worldSampleLabel
            // 
            this.worldSampleLabel.AutoSize = true;
            this.worldSampleLabel.Location = new Point(198, 302);
            this.worldSampleLabel.Name = "worldSampleLabel";
            this.worldSampleLabel.Size = new Size(19, 15);
            this.worldSampleLabel.TabIndex = 20;
            this.worldSampleLabel.Text = "...";
            // 
            // linkLabel10
            // 
            linkLabel10.AutoSize = true;
            linkLabel10.Location = new Point(35, 302);
            linkLabel10.Name = "linkLabel10";
            linkLabel10.Size = new Size(53, 15);
            linkLabel10.TabIndex = 19;
            linkLabel10.TabStop = true;
            linkLabel10.Text = "{world}";
            variableToolTip.SetToolTip(linkLabel10, "キャラクターの現在いるワールド名");
            // 
            // placeSampleLabel
            // 
            this.placeSampleLabel.AutoSize = true;
            this.placeSampleLabel.Location = new Point(198, 134);
            this.placeSampleLabel.Name = "placeSampleLabel";
            this.placeSampleLabel.Size = new Size(19, 15);
            this.placeSampleLabel.TabIndex = 16;
            this.placeSampleLabel.Text = "...";
            // 
            // linkLabel8
            // 
            linkLabel8.AutoSize = true;
            linkLabel8.Location = new Point(35, 134);
            linkLabel8.Name = "linkLabel8";
            linkLabel8.Size = new Size(51, 15);
            linkLabel8.TabIndex = 15;
            linkLabel8.TabStop = true;
            linkLabel8.Text = "{place}";
            variableToolTip.SetToolTip(linkLabel8, "キャラクターの現在いるエリアの地名");
            // 
            // regionSampleLabel
            // 
            this.regionSampleLabel.AutoSize = true;
            this.regionSampleLabel.Location = new Point(198, 110);
            this.regionSampleLabel.Name = "regionSampleLabel";
            this.regionSampleLabel.Size = new Size(19, 15);
            this.regionSampleLabel.TabIndex = 14;
            this.regionSampleLabel.Text = "...";
            // 
            // linkLabel7
            // 
            linkLabel7.AutoSize = true;
            linkLabel7.Location = new Point(35, 110);
            linkLabel7.Name = "linkLabel7";
            linkLabel7.Size = new Size(57, 15);
            linkLabel7.TabIndex = 13;
            linkLabel7.TabStop = true;
            linkLabel7.Text = "{region}";
            variableToolTip.SetToolTip(linkLabel7, "キャラクターの現在いるエリアの地域名");
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 20);
            label5.Name = "label5";
            label5.Size = new Size(299, 30);
            label5.TabIndex = 12;
            label5.Text = "フォーマットに使用できる変数の値が表示されます。\r\n空欄になっている変数は現在利用できないことを示しています。";
            // 
            // jobNameSampleLabel
            // 
            this.jobNameSampleLabel.AutoSize = true;
            this.jobNameSampleLabel.Location = new Point(198, 278);
            this.jobNameSampleLabel.Name = "jobNameSampleLabel";
            this.jobNameSampleLabel.Size = new Size(19, 15);
            this.jobNameSampleLabel.TabIndex = 11;
            this.jobNameSampleLabel.Text = "...";
            // 
            // linkLabel6
            // 
            linkLabel6.AutoSize = true;
            linkLabel6.Location = new Point(35, 278);
            linkLabel6.Name = "linkLabel6";
            linkLabel6.Size = new Size(80, 15);
            linkLabel6.TabIndex = 10;
            linkLabel6.TabStop = true;
            linkLabel6.Text = "{job_name}";
            variableToolTip.SetToolTip(linkLabel6, "キャラクターの現在のクラス / ジョブ名");
            // 
            // levelSampleLabel
            // 
            this.levelSampleLabel.AutoSize = true;
            this.levelSampleLabel.Location = new Point(198, 230);
            this.levelSampleLabel.Name = "levelSampleLabel";
            this.levelSampleLabel.Size = new Size(19, 15);
            this.levelSampleLabel.TabIndex = 9;
            this.levelSampleLabel.Text = "...";
            // 
            // jobSampleLabel
            // 
            this.jobSampleLabel.AutoSize = true;
            this.jobSampleLabel.Location = new Point(198, 254);
            this.jobSampleLabel.Name = "jobSampleLabel";
            this.jobSampleLabel.Size = new Size(19, 15);
            this.jobSampleLabel.TabIndex = 7;
            this.jobSampleLabel.Text = "...";
            // 
            // linkLabel5
            // 
            linkLabel5.AutoSize = true;
            linkLabel5.Location = new Point(35, 230);
            linkLabel5.Name = "linkLabel5";
            linkLabel5.Size = new Size(48, 15);
            linkLabel5.TabIndex = 8;
            linkLabel5.TabStop = true;
            linkLabel5.Text = "{level}";
            variableToolTip.SetToolTip(linkLabel5, "キャラクターの現在のクラス / ジョブのレベル");
            // 
            // linkLabel4
            // 
            linkLabel4.AutoSize = true;
            linkLabel4.Location = new Point(35, 254);
            linkLabel4.Name = "linkLabel4";
            linkLabel4.Size = new Size(40, 15);
            linkLabel4.TabIndex = 6;
            linkLabel4.TabStop = true;
            linkLabel4.Text = "{job}";
            variableToolTip.SetToolTip(linkLabel4, "キャラクターの現在のクラス / ジョブ略称");
            // 
            // characterSampleLabel
            // 
            this.characterSampleLabel.AutoSize = true;
            this.characterSampleLabel.Location = new Point(198, 206);
            this.characterSampleLabel.Name = "characterSampleLabel";
            this.characterSampleLabel.Size = new Size(19, 15);
            this.characterSampleLabel.TabIndex = 5;
            this.characterSampleLabel.Text = "...";
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Location = new Point(35, 206);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(76, 15);
            linkLabel3.TabIndex = 4;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "{character}";
            variableToolTip.SetToolTip(linkLabel3, "キャラクター名");
            // 
            // actZoneSampleLabel
            // 
            this.actZoneSampleLabel.AutoSize = true;
            this.actZoneSampleLabel.Location = new Point(198, 86);
            this.actZoneSampleLabel.Name = "actZoneSampleLabel";
            this.actZoneSampleLabel.Size = new Size(19, 15);
            this.actZoneSampleLabel.TabIndex = 3;
            this.actZoneSampleLabel.Text = "...";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new Point(35, 86);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(73, 15);
            linkLabel2.TabIndex = 2;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "{act_zone}";
            variableToolTip.SetToolTip(linkLabel2, "キャラクターの現在いるエリアの名前 (言語設定に関わらず常に英語です)");
            // 
            // statusSampleLabel
            // 
            this.statusSampleLabel.AutoSize = true;
            this.statusSampleLabel.Location = new Point(198, 62);
            this.statusSampleLabel.Name = "statusSampleLabel";
            this.statusSampleLabel.Size = new Size(19, 15);
            this.statusSampleLabel.TabIndex = 1;
            this.statusSampleLabel.Text = "...";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(35, 62);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(57, 15);
            linkLabel1.TabIndex = 0;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "{status}";
            variableToolTip.SetToolTip(linkLabel1, "キャラクターの現在のオンラインステータス");
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(71, 93);
            label11.Name = "label11";
            label11.Size = new Size(33, 15);
            label11.TabIndex = 11;
            label11.Text = "Text";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(26, 30);
            label12.Name = "label12";
            label12.Size = new Size(123, 15);
            label12.TabIndex = 15;
            label12.Text = "Authorization Token";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(24, 135);
            label13.Name = "label13";
            label13.Size = new Size(78, 15);
            label13.TabIndex = 17;
            label13.Text = "Logo Tooltip";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(24, 73);
            label14.Name = "label14";
            label14.Size = new Size(97, 15);
            label14.TabIndex = 21;
            label14.Text = "Details (戦闘時)";
            // 
            // formatGroup
            // 
            formatGroup.Controls.Add(this.requireTargetingOnCombatCheckBox);
            formatGroup.Controls.Add(label14);
            formatGroup.Controls.Add(this.detailsInCombatTextBox);
            formatGroup.Controls.Add(this.resetTimerCheckBox);
            formatGroup.Controls.Add(this.logoTooltipTextBox);
            formatGroup.Controls.Add(label13);
            formatGroup.Controls.Add(this.jobIconCheckBox);
            formatGroup.Controls.Add(this.tooltipTextBox);
            formatGroup.Controls.Add(this.stateTextBox);
            formatGroup.Controls.Add(label3);
            formatGroup.Controls.Add(label2);
            formatGroup.Controls.Add(label1);
            formatGroup.Controls.Add(this.detailsTextBox);
            formatGroup.Location = new Point(28, 167);
            formatGroup.Margin = new Padding(3, 4, 3, 4);
            formatGroup.Name = "formatGroup";
            formatGroup.Padding = new Padding(3, 4, 3, 4);
            formatGroup.Size = new Size(548, 303);
            formatGroup.TabIndex = 2;
            formatGroup.TabStop = false;
            formatGroup.Text = "Rich Presence";
            // 
            // requireTargetingOnCombatCheckBox
            // 
            this.requireTargetingOnCombatCheckBox.AutoSize = true;
            this.requireTargetingOnCombatCheckBox.Location = new Point(26, 276);
            this.requireTargetingOnCombatCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.requireTargetingOnCombatCheckBox.Name = "requireTargetingOnCombatCheckBox";
            this.requireTargetingOnCombatCheckBox.Size = new Size(242, 19);
            this.requireTargetingOnCombatCheckBox.TabIndex = 20;
            this.requireTargetingOnCombatCheckBox.Text = "ターゲット時にのみ戦闘中の判定を有効にする";
            this.requireTargetingOnCombatCheckBox.UseVisualStyleBackColor = true;
            // 
            // detailsInCombatTextBox
            // 
            this.detailsInCombatTextBox.Location = new Point(234, 69);
            this.detailsInCombatTextBox.Margin = new Padding(3, 4, 3, 4);
            this.detailsInCombatTextBox.Name = "detailsInCombatTextBox";
            this.detailsInCombatTextBox.Size = new Size(280, 23);
            this.detailsInCombatTextBox.TabIndex = 20;
            this.detailsInCombatTextBox.Text = "{target} (HP: {hpp}%)";
            // 
            // resetTimerCheckBox
            // 
            this.resetTimerCheckBox.AutoSize = true;
            this.resetTimerCheckBox.Checked = true;
            this.resetTimerCheckBox.CheckState = CheckState.Checked;
            this.resetTimerCheckBox.Location = new Point(27, 249);
            this.resetTimerCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.resetTimerCheckBox.Name = "resetTimerCheckBox";
            this.resetTimerCheckBox.Size = new Size(261, 19);
            this.resetTimerCheckBox.TabIndex = 19;
            this.resetTimerCheckBox.Text = "エリアチェンジ時に経過時間のタイマーをリセットする";
            this.resetTimerCheckBox.UseVisualStyleBackColor = true;
            // 
            // logoTooltipTextBox
            // 
            this.logoTooltipTextBox.Location = new Point(234, 132);
            this.logoTooltipTextBox.Margin = new Padding(3, 4, 3, 4);
            this.logoTooltipTextBox.Name = "logoTooltipTextBox";
            this.logoTooltipTextBox.Size = new Size(280, 23);
            this.logoTooltipTextBox.TabIndex = 18;
            this.logoTooltipTextBox.Text = "FINAL FANTASY XIV: Shadowbringers";
            // 
            // jobIconCheckBox
            // 
            this.jobIconCheckBox.AutoSize = true;
            this.jobIconCheckBox.Checked = true;
            this.jobIconCheckBox.CheckState = CheckState.Checked;
            this.jobIconCheckBox.Location = new Point(26, 166);
            this.jobIconCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.jobIconCheckBox.Name = "jobIconCheckBox";
            this.jobIconCheckBox.Size = new Size(138, 19);
            this.jobIconCheckBox.TabIndex = 10;
            this.jobIconCheckBox.Text = "ジョブアイコンを表示する";
            this.jobIconCheckBox.UseVisualStyleBackColor = true;
            this.jobIconCheckBox.CheckedChanged += new EventHandler(this.jobIconCheckBox_CheckedChanged);
            // 
            // tooltipTextBox
            // 
            this.tooltipTextBox.Location = new Point(234, 202);
            this.tooltipTextBox.Margin = new Padding(3, 4, 3, 4);
            this.tooltipTextBox.Name = "tooltipTextBox";
            this.tooltipTextBox.Size = new Size(280, 23);
            this.tooltipTextBox.TabIndex = 7;
            this.tooltipTextBox.Text = "{job} Lv{level}";
            // 
            // stateTextBox
            // 
            this.stateTextBox.Location = new Point(234, 101);
            this.stateTextBox.Margin = new Padding(3, 4, 3, 4);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new Size(280, 23);
            this.stateTextBox.TabIndex = 5;
            this.stateTextBox.Text = "{place} @ {world}";
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Location = new Point(234, 38);
            this.detailsTextBox.Margin = new Padding(3, 4, 3, 4);
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.Size = new Size(280, 23);
            this.detailsTextBox.TabIndex = 0;
            this.detailsTextBox.Text = "{party} / {status}";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.onlineStatusEmojiCheckBox);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(this.customStatusOnContentsTextBox);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(this.emojiIdTextBox);
            groupBox1.Controls.Add(this.customStatusCheckBox);
            groupBox1.Controls.Add(this.customStatusTextDefaultTextBox);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(this.customStatusTextBox);
            groupBox1.Controls.Add(this.jobEmojiCheckBox);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(this.authorizationTokenTextBox);
            groupBox1.Location = new Point(28, 480);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(546, 289);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Custom Status";
            // 
            // onlineStatusEmojiCheckBox
            // 
            this.onlineStatusEmojiCheckBox.AutoSize = true;
            this.onlineStatusEmojiCheckBox.Checked = true;
            this.onlineStatusEmojiCheckBox.CheckState = CheckState.Checked;
            this.onlineStatusEmojiCheckBox.Location = new Point(74, 184);
            this.onlineStatusEmojiCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.onlineStatusEmojiCheckBox.Name = "onlineStatusEmojiCheckBox";
            this.onlineStatusEmojiCheckBox.Size = new Size(213, 19);
            this.onlineStatusEmojiCheckBox.TabIndex = 19;
            this.onlineStatusEmojiCheckBox.Text = "オンラインステータスの絵文字を表示する";
            this.onlineStatusEmojiCheckBox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(71, 125);
            label9.Name = "label9";
            label9.Size = new Size(103, 15);
            label9.TabIndex = 17;
            label9.Text = "Text (コンテンツ中)";
            // 
            // customStatusOnContentsTextBox
            // 
            this.customStatusOnContentsTextBox.Location = new Point(234, 122);
            this.customStatusOnContentsTextBox.Margin = new Padding(3, 4, 3, 4);
            this.customStatusOnContentsTextBox.Name = "customStatusOnContentsTextBox";
            this.customStatusOnContentsTextBox.Size = new Size(280, 23);
            this.customStatusOnContentsTextBox.TabIndex = 18;
            this.customStatusOnContentsTextBox.Text = "{place}";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(71, 220);
            label8.Name = "label8";
            label8.Size = new Size(115, 15);
            label8.TabIndex = 13;
            label8.Text = "emoji_id (デフォルト)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(71, 257);
            label4.Name = "label4";
            label4.Size = new Size(91, 15);
            label4.TabIndex = 13;
            label4.Text = "Text (デフォルト)";
            // 
            // emojiIdTextBox
            // 
            this.emojiIdTextBox.Location = new Point(234, 217);
            this.emojiIdTextBox.Margin = new Padding(3, 4, 3, 4);
            this.emojiIdTextBox.Name = "emojiIdTextBox";
            this.emojiIdTextBox.Size = new Size(280, 23);
            this.emojiIdTextBox.TabIndex = 14;
            // 
            // customStatusCheckBox
            // 
            this.customStatusCheckBox.AutoSize = true;
            this.customStatusCheckBox.Checked = true;
            this.customStatusCheckBox.CheckState = CheckState.Checked;
            this.customStatusCheckBox.Location = new Point(28, 67);
            this.customStatusCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.customStatusCheckBox.Name = "customStatusCheckBox";
            this.customStatusCheckBox.Size = new Size(140, 19);
            this.customStatusCheckBox.TabIndex = 13;
            this.customStatusCheckBox.Text = "カスタムステータスを表示";
            this.customStatusCheckBox.UseVisualStyleBackColor = true;
            this.customStatusCheckBox.CheckedChanged += new EventHandler(this.customStatusCheckBox_CheckedChanged);
            // 
            // customStatusTextDefaultTextBox
            // 
            this.customStatusTextDefaultTextBox.Location = new Point(234, 254);
            this.customStatusTextDefaultTextBox.Margin = new Padding(3, 4, 3, 4);
            this.customStatusTextDefaultTextBox.Name = "customStatusTextDefaultTextBox";
            this.customStatusTextDefaultTextBox.Size = new Size(280, 23);
            this.customStatusTextDefaultTextBox.TabIndex = 14;
            // 
            // customStatusTextBox
            // 
            this.customStatusTextBox.Location = new Point(234, 90);
            this.customStatusTextBox.Margin = new Padding(3, 4, 3, 4);
            this.customStatusTextBox.Name = "customStatusTextBox";
            this.customStatusTextBox.Size = new Size(280, 23);
            this.customStatusTextBox.TabIndex = 12;
            // 
            // jobEmojiCheckBox
            // 
            this.jobEmojiCheckBox.AutoSize = true;
            this.jobEmojiCheckBox.Checked = true;
            this.jobEmojiCheckBox.CheckState = CheckState.Checked;
            this.jobEmojiCheckBox.Location = new Point(74, 157);
            this.jobEmojiCheckBox.Margin = new Padding(3, 4, 3, 4);
            this.jobEmojiCheckBox.Name = "jobEmojiCheckBox";
            this.jobEmojiCheckBox.Size = new Size(184, 19);
            this.jobEmojiCheckBox.TabIndex = 14;
            this.jobEmojiCheckBox.Text = "ジョブアイコンの絵文字を表示する";
            this.jobEmojiCheckBox.UseVisualStyleBackColor = true;
            // 
            // authorizationTokenTextBox
            // 
            this.authorizationTokenTextBox.Location = new Point(189, 27);
            this.authorizationTokenTextBox.Margin = new Padding(3, 4, 3, 4);
            this.authorizationTokenTextBox.Name = "authorizationTokenTextBox";
            this.authorizationTokenTextBox.Size = new Size(280, 23);
            this.authorizationTokenTextBox.TabIndex = 16;
            this.authorizationTokenTextBox.TextChanged += new EventHandler(this.authorizationTokenTextBox_TextChanged);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.sharlayanLabel);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(this.processLabel);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new Point(28, 18);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(547, 137);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "ステータス";
            // 
            // sharlayanLabel
            // 
            this.sharlayanLabel.AutoSize = true;
            this.sharlayanLabel.Location = new Point(190, 77);
            this.sharlayanLabel.Name = "sharlayanLabel";
            this.sharlayanLabel.Size = new Size(52, 15);
            this.sharlayanLabel.TabIndex = 3;
            this.sharlayanLabel.Text = "未アタッチ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(32, 77);
            label7.Name = "label7";
            label7.Size = new Size(70, 15);
            label7.TabIndex = 2;
            label7.Text = "Sharlayan:";
            // 
            // processLabel
            // 
            this.processLabel.Location = new Point(190, 40);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new Size(185, 15);
            this.processLabel.TabIndex = 1;
            this.processLabel.Text = "なし";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(32, 40);
            label6.Name = "label6";
            label6.Size = new Size(91, 15);
            label6.TabIndex = 0;
            label6.Text = "検出したプロセス:";
            // 
            // PluginTabControl
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(formatGroup);
            this.Controls.Add(variablesGroupBox);
            this.Font = new Font("Meiryo UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "PluginTabControl";
            this.Size = new Size(1063, 787);
            this.Load += new EventHandler(this.UserControl1_Load);
            variablesGroupBox.ResumeLayout(false);
            variablesGroupBox.PerformLayout();
            formatGroup.ResumeLayout(false);
            formatGroup.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public TextBox detailsTextBox;
        public TextBox tooltipTextBox;
        public TextBox stateTextBox;
        public CheckBox jobIconCheckBox;
        public TextBox authorizationTokenTextBox;
        public CheckBox jobEmojiCheckBox;
        public CheckBox customStatusCheckBox;
        public TextBox customStatusTextBox;
        public TextBox logoTooltipTextBox;
        public CheckBox resetTimerCheckBox;
        public TextBox detailsInCombatTextBox;
        internal Label homeWorldSampleLabel;
        internal Label worldSampleLabel;
        internal Label placeSampleLabel;
        internal Label regionSampleLabel;
        internal Label jobNameSampleLabel;
        internal Label levelSampleLabel;
        internal Label jobSampleLabel;
        internal Label characterSampleLabel;
        internal Label actZoneSampleLabel;
        internal Label statusSampleLabel;
        internal Label partySampleLabel;
        internal Label zSampleLabel;
        internal Label ySampleLabel;
        internal Label xSampleLabel;
        internal Label targetSampleLabel;
        internal Label thppSampleLabel;
        internal Label thpSampleLabel;
        internal Label mppSampleLabel;
        internal Label mpSampleLabel;
        internal Label hppSampleLabel;
        internal Label hpSampleLabel;
        internal Label processLabel;
        internal Label sharlayanLabel;
        public TextBox emojiIdTextBox;
        public TextBox customStatusTextDefaultTextBox;
        public TextBox customStatusOnContentsTextBox;
        public CheckBox onlineStatusEmojiCheckBox;
        internal Label dutySampleLabel;
        public CheckBox requireTargetingOnCombatCheckBox;
        internal Label zoneSampleLabel;
    }
}
