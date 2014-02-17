/*
	Copyright 2010 MCSharp Team Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/
namespace Overv.Gui
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Window ) );
            this.txtServerLogs = new System.Windows.Forms.TextBox();
            this.liClients = new System.Windows.Forms.ListBox();
            this.gbCTFLogs = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCTFLogs = new System.Windows.Forms.TextBox();
            this.lable1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnBan = new System.Windows.Forms.Button();
            this.btnKickban = new System.Windows.Forms.Button();
            this.btnBanIp = new System.Windows.Forms.Button();
            this.btnChangeRank = new System.Windows.Forms.Button();
            this.gbPlayerControls = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpMain = new System.Windows.Forms.TabPage();
            this.gbServerLogs = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tpCTF = new System.Windows.Forms.TabPage();
            this.lbCurrentLevel = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbBlueWins = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.liBluePlayers = new System.Windows.Forms.ListBox();
            this.lbBlueFlagStatus = new System.Windows.Forms.Label();
            this.lbBlueScore = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbRedWins = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.liRedPlayers = new System.Windows.Forms.ListBox();
            this.lbRedFlagStatus = new System.Windows.Forms.Label();
            this.lbRedScore = new System.Windows.Forms.Label();
            this.tpMaps = new System.Windows.Forms.TabPage();
            this.gbMapPreview = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbMapInfo = new System.Windows.Forms.GroupBox();
            this.txtMapDescription = new System.Windows.Forms.TextBox();
            this.lblMapDescription = new System.Windows.Forms.Label();
            this.lblMapAuthor = new System.Windows.Forms.Label();
            this.lblMapName = new System.Windows.Forms.Label();
            this.gbOnlineMaps = new System.Windows.Forms.GroupBox();
            this.btnDownloadMap = new System.Windows.Forms.Button();
            this.liOnlineMaps = new System.Windows.Forms.ListBox();
            this.gbCTFLogs.SuspendLayout();
            this.gbPlayerControls.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpMain.SuspendLayout();
            this.gbServerLogs.SuspendLayout();
            this.tpCTF.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tpMaps.SuspendLayout();
            this.gbMapPreview.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).BeginInit();
            this.gbMapInfo.SuspendLayout();
            this.gbOnlineMaps.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServerLogs
            // 
            this.txtServerLogs.BackColor = System.Drawing.SystemColors.Window;
            this.txtServerLogs.Location = new System.Drawing.Point( 6, 19 );
            this.txtServerLogs.Multiline = true;
            this.txtServerLogs.Name = "txtServerLogs";
            this.txtServerLogs.ReadOnly = true;
            this.txtServerLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtServerLogs.Size = new System.Drawing.Size( 434, 273 );
            this.txtServerLogs.TabIndex = 0;
            // 
            // liClients
            // 
            this.liClients.FormattingEnabled = true;
            this.liClients.Location = new System.Drawing.Point( 461, 32 );
            this.liClients.Name = "liClients";
            this.liClients.Size = new System.Drawing.Size( 129, 420 );
            this.liClients.TabIndex = 1;
            // 
            // gbCTFLogs
            // 
            this.gbCTFLogs.Controls.Add( this.button2 );
            this.gbCTFLogs.Controls.Add( this.txtCTFLogs );
            this.gbCTFLogs.Location = new System.Drawing.Point( 9, 336 );
            this.gbCTFLogs.Name = "gbCTFLogs";
            this.gbCTFLogs.Size = new System.Drawing.Size( 267, 116 );
            this.gbCTFLogs.TabIndex = 2;
            this.gbCTFLogs.TabStop = false;
            this.gbCTFLogs.Text = "CTF Logs";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point( 241, 19 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 20, 91 );
            this.button2.TabIndex = 13;
            this.button2.Text = ">";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler( this.button2_Click );
            // 
            // txtCTFLogs
            // 
            this.txtCTFLogs.BackColor = System.Drawing.SystemColors.Window;
            this.txtCTFLogs.Location = new System.Drawing.Point( 6, 19 );
            this.txtCTFLogs.Multiline = true;
            this.txtCTFLogs.Name = "txtCTFLogs";
            this.txtCTFLogs.ReadOnly = true;
            this.txtCTFLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCTFLogs.Size = new System.Drawing.Size( 229, 91 );
            this.txtCTFLogs.TabIndex = 0;
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point( 6, 9 );
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size( 32, 13 );
            this.lable1.TabIndex = 3;
            this.lable1.Text = "URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.BackColor = System.Drawing.Color.White;
            this.txtUrl.Location = new System.Drawing.Point( 44, 6 );
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ReadOnly = true;
            this.txtUrl.Size = new System.Drawing.Size( 465, 20 );
            this.txtUrl.TabIndex = 4;
            // 
            // btnKick
            // 
            this.btnKick.Location = new System.Drawing.Point( 6, 19 );
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size( 74, 23 );
            this.btnKick.TabIndex = 5;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler( this.btnKick_Click );
            // 
            // btnBan
            // 
            this.btnBan.Location = new System.Drawing.Point( 93, 19 );
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size( 74, 23 );
            this.btnBan.TabIndex = 6;
            this.btnBan.Text = "Ban";
            this.btnBan.UseVisualStyleBackColor = true;
            this.btnBan.Click += new System.EventHandler( this.btnBan_Click );
            // 
            // btnKickban
            // 
            this.btnKickban.Location = new System.Drawing.Point( 6, 53 );
            this.btnKickban.Name = "btnKickban";
            this.btnKickban.Size = new System.Drawing.Size( 74, 23 );
            this.btnKickban.TabIndex = 7;
            this.btnKickban.Text = "Kickban";
            this.btnKickban.UseVisualStyleBackColor = true;
            this.btnKickban.Click += new System.EventHandler( this.btnKickban_Click );
            // 
            // btnBanIp
            // 
            this.btnBanIp.Location = new System.Drawing.Point( 93, 53 );
            this.btnBanIp.Name = "btnBanIp";
            this.btnBanIp.Size = new System.Drawing.Size( 74, 23 );
            this.btnBanIp.TabIndex = 8;
            this.btnBanIp.Text = "BanIP";
            this.btnBanIp.UseVisualStyleBackColor = true;
            this.btnBanIp.Click += new System.EventHandler( this.btnBanIp_Click );
            // 
            // btnChangeRank
            // 
            this.btnChangeRank.Location = new System.Drawing.Point( 6, 87 );
            this.btnChangeRank.Name = "btnChangeRank";
            this.btnChangeRank.Size = new System.Drawing.Size( 161, 23 );
            this.btnChangeRank.TabIndex = 9;
            this.btnChangeRank.Text = "Change Rank";
            this.btnChangeRank.UseVisualStyleBackColor = true;
            this.btnChangeRank.Click += new System.EventHandler( this.btnChangeRank_Click );
            // 
            // gbPlayerControls
            // 
            this.gbPlayerControls.Controls.Add( this.btnKick );
            this.gbPlayerControls.Controls.Add( this.btnChangeRank );
            this.gbPlayerControls.Controls.Add( this.btnBan );
            this.gbPlayerControls.Controls.Add( this.btnBanIp );
            this.gbPlayerControls.Controls.Add( this.btnKickban );
            this.gbPlayerControls.Location = new System.Drawing.Point( 282, 336 );
            this.gbPlayerControls.Name = "gbPlayerControls";
            this.gbPlayerControls.Size = new System.Drawing.Size( 173, 116 );
            this.gbPlayerControls.TabIndex = 10;
            this.gbPlayerControls.TabStop = false;
            this.gbPlayerControls.Text = "Player Controls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 6, 461 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 32, 13 );
            this.label1.TabIndex = 11;
            this.label1.Text = "Chat:";
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point( 44, 458 );
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size( 546, 20 );
            this.txtChat.TabIndex = 12;
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtInput_KeyDown );
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.tpMain );
            this.tabControl1.Controls.Add( this.tpCTF );
            this.tabControl1.Controls.Add( this.tpMaps );
            this.tabControl1.Location = new System.Drawing.Point( 12, 12 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 604, 509 );
            this.tabControl1.TabIndex = 13;
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add( this.gbServerLogs );
            this.tpMain.Controls.Add( this.label1 );
            this.tpMain.Controls.Add( this.txtChat );
            this.tpMain.Controls.Add( this.button1 );
            this.tpMain.Controls.Add( this.txtUrl );
            this.tpMain.Controls.Add( this.gbCTFLogs );
            this.tpMain.Controls.Add( this.gbPlayerControls );
            this.tpMain.Controls.Add( this.lable1 );
            this.tpMain.Controls.Add( this.liClients );
            this.tpMain.Location = new System.Drawing.Point( 4, 22 );
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding( 3 );
            this.tpMain.Size = new System.Drawing.Size( 596, 483 );
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Main";
            // 
            // gbServerLogs
            // 
            this.gbServerLogs.Controls.Add( this.txtServerLogs );
            this.gbServerLogs.Location = new System.Drawing.Point( 9, 32 );
            this.gbServerLogs.Name = "gbServerLogs";
            this.gbServerLogs.Size = new System.Drawing.Size( 446, 298 );
            this.gbServerLogs.TabIndex = 6;
            this.gbServerLogs.TabStop = false;
            this.gbServerLogs.Text = "Server Logs";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 515, 5 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 75, 22 );
            this.button1.TabIndex = 5;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tpCTF
            // 
            this.tpCTF.Controls.Add( this.lbCurrentLevel );
            this.tpCTF.Controls.Add( this.groupBox5 );
            this.tpCTF.Controls.Add( this.groupBox4 );
            this.tpCTF.Location = new System.Drawing.Point( 4, 22 );
            this.tpCTF.Name = "tpCTF";
            this.tpCTF.Padding = new System.Windows.Forms.Padding( 3 );
            this.tpCTF.Size = new System.Drawing.Size( 596, 483 );
            this.tpCTF.TabIndex = 1;
            this.tpCTF.Text = "Game Overview";
            // 
            // lbCurrentLevel
            // 
            this.lbCurrentLevel.AutoSize = true;
            this.lbCurrentLevel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbCurrentLevel.Location = new System.Drawing.Point( 6, 9 );
            this.lbCurrentLevel.Name = "lbCurrentLevel";
            this.lbCurrentLevel.Size = new System.Drawing.Size( 216, 25 );
            this.lbCurrentLevel.TabIndex = 2;
            this.lbCurrentLevel.Text = "Currently playing on: ";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add( this.lbBlueWins );
            this.groupBox5.Controls.Add( this.groupBox7 );
            this.groupBox5.Controls.Add( this.lbBlueFlagStatus );
            this.groupBox5.Controls.Add( this.lbBlueScore );
            this.groupBox5.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.groupBox5.Location = new System.Drawing.Point( 303, 43 );
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size( 284, 430 );
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Blue Team";
            // 
            // lbBlueWins
            // 
            this.lbBlueWins.AutoSize = true;
            this.lbBlueWins.Font = new System.Drawing.Font( "Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbBlueWins.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbBlueWins.Location = new System.Drawing.Point( 151, 16 );
            this.lbBlueWins.Name = "lbBlueWins";
            this.lbBlueWins.Size = new System.Drawing.Size( 96, 33 );
            this.lbBlueWins.TabIndex = 4;
            this.lbBlueWins.Text = "Wins: ";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add( this.liBluePlayers );
            this.groupBox7.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.groupBox7.Location = new System.Drawing.Point( 12, 96 );
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size( 260, 322 );
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Players";
            // 
            // liBluePlayers
            // 
            this.liBluePlayers.Font = new System.Drawing.Font( "Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.liBluePlayers.FormattingEnabled = true;
            this.liBluePlayers.ItemHeight = 24;
            this.liBluePlayers.Location = new System.Drawing.Point( 6, 19 );
            this.liBluePlayers.Name = "liBluePlayers";
            this.liBluePlayers.Size = new System.Drawing.Size( 248, 292 );
            this.liBluePlayers.TabIndex = 1;
            // 
            // lbBlueFlagStatus
            // 
            this.lbBlueFlagStatus.AutoSize = true;
            this.lbBlueFlagStatus.Font = new System.Drawing.Font( "Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbBlueFlagStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbBlueFlagStatus.Location = new System.Drawing.Point( 7, 58 );
            this.lbBlueFlagStatus.Name = "lbBlueFlagStatus";
            this.lbBlueFlagStatus.Size = new System.Drawing.Size( 60, 25 );
            this.lbBlueFlagStatus.TabIndex = 3;
            this.lbBlueFlagStatus.Text = "Flag:";
            // 
            // lbBlueScore
            // 
            this.lbBlueScore.AutoSize = true;
            this.lbBlueScore.Font = new System.Drawing.Font( "Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbBlueScore.ForeColor = System.Drawing.Color.SteelBlue;
            this.lbBlueScore.Location = new System.Drawing.Point( 6, 16 );
            this.lbBlueScore.Name = "lbBlueScore";
            this.lbBlueScore.Size = new System.Drawing.Size( 107, 33 );
            this.lbBlueScore.TabIndex = 3;
            this.lbBlueScore.Text = "Score: ";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add( this.lbRedWins );
            this.groupBox4.Controls.Add( this.groupBox6 );
            this.groupBox4.Controls.Add( this.lbRedFlagStatus );
            this.groupBox4.Controls.Add( this.lbRedScore );
            this.groupBox4.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.groupBox4.Location = new System.Drawing.Point( 9, 43 );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size( 284, 430 );
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Red Team";
            // 
            // lbRedWins
            // 
            this.lbRedWins.AutoSize = true;
            this.lbRedWins.Font = new System.Drawing.Font( "Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbRedWins.ForeColor = System.Drawing.Color.Maroon;
            this.lbRedWins.Location = new System.Drawing.Point( 150, 16 );
            this.lbRedWins.Name = "lbRedWins";
            this.lbRedWins.Size = new System.Drawing.Size( 96, 33 );
            this.lbRedWins.TabIndex = 3;
            this.lbRedWins.Text = "Wins: ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add( this.liRedPlayers );
            this.groupBox6.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.groupBox6.Location = new System.Drawing.Point( 12, 96 );
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size( 260, 322 );
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Players";
            // 
            // liRedPlayers
            // 
            this.liRedPlayers.Font = new System.Drawing.Font( "Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.liRedPlayers.FormattingEnabled = true;
            this.liRedPlayers.ItemHeight = 24;
            this.liRedPlayers.Location = new System.Drawing.Point( 6, 19 );
            this.liRedPlayers.Name = "liRedPlayers";
            this.liRedPlayers.Size = new System.Drawing.Size( 248, 292 );
            this.liRedPlayers.TabIndex = 0;
            // 
            // lbRedFlagStatus
            // 
            this.lbRedFlagStatus.AutoSize = true;
            this.lbRedFlagStatus.Font = new System.Drawing.Font( "Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbRedFlagStatus.ForeColor = System.Drawing.Color.Maroon;
            this.lbRedFlagStatus.Location = new System.Drawing.Point( 7, 58 );
            this.lbRedFlagStatus.Name = "lbRedFlagStatus";
            this.lbRedFlagStatus.Size = new System.Drawing.Size( 60, 25 );
            this.lbRedFlagStatus.TabIndex = 2;
            this.lbRedFlagStatus.Text = "Flag:";
            // 
            // lbRedScore
            // 
            this.lbRedScore.AutoSize = true;
            this.lbRedScore.Font = new System.Drawing.Font( "Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lbRedScore.ForeColor = System.Drawing.Color.Maroon;
            this.lbRedScore.Location = new System.Drawing.Point( 6, 16 );
            this.lbRedScore.Name = "lbRedScore";
            this.lbRedScore.Size = new System.Drawing.Size( 107, 33 );
            this.lbRedScore.TabIndex = 2;
            this.lbRedScore.Text = "Score: ";
            // 
            // tpMaps
            // 
            this.tpMaps.Controls.Add( this.gbMapPreview );
            this.tpMaps.Controls.Add( this.gbMapInfo );
            this.tpMaps.Controls.Add( this.gbOnlineMaps );
            this.tpMaps.Location = new System.Drawing.Point( 4, 22 );
            this.tpMaps.Name = "tpMaps";
            this.tpMaps.Padding = new System.Windows.Forms.Padding( 3 );
            this.tpMaps.Size = new System.Drawing.Size( 596, 483 );
            this.tpMaps.TabIndex = 2;
            this.tpMaps.Text = "Maps";
            this.tpMaps.UseVisualStyleBackColor = true;
            // 
            // gbMapPreview
            // 
            this.gbMapPreview.Controls.Add( this.pictureBox1 );
            this.gbMapPreview.Location = new System.Drawing.Point( 193, 230 );
            this.gbMapPreview.Name = "gbMapPreview";
            this.gbMapPreview.Size = new System.Drawing.Size( 397, 247 );
            this.gbMapPreview.TabIndex = 3;
            this.gbMapPreview.TabStop = false;
            this.gbMapPreview.Text = "Map Preview";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ErrorImage = ( (System.Drawing.Image)( resources.GetObject( "pictureBox1.ErrorImage" ) ) );
            this.pictureBox1.InitialImage = ( (System.Drawing.Image)( resources.GetObject( "pictureBox1.InitialImage" ) ) );
            this.pictureBox1.Location = new System.Drawing.Point( 84, 17 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size( 222, 222 );
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // gbMapInfo
            // 
            this.gbMapInfo.Controls.Add( this.txtMapDescription );
            this.gbMapInfo.Controls.Add( this.lblMapDescription );
            this.gbMapInfo.Controls.Add( this.lblMapAuthor );
            this.gbMapInfo.Controls.Add( this.lblMapName );
            this.gbMapInfo.Location = new System.Drawing.Point( 193, 6 );
            this.gbMapInfo.Name = "gbMapInfo";
            this.gbMapInfo.Size = new System.Drawing.Size( 397, 218 );
            this.gbMapInfo.TabIndex = 2;
            this.gbMapInfo.TabStop = false;
            this.gbMapInfo.Text = "Map Info";
            // 
            // txtMapDescription
            // 
            this.txtMapDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMapDescription.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.txtMapDescription.ForeColor = System.Drawing.Color.Gray;
            this.txtMapDescription.Location = new System.Drawing.Point( 17, 116 );
            this.txtMapDescription.Multiline = true;
            this.txtMapDescription.Name = "txtMapDescription";
            this.txtMapDescription.Size = new System.Drawing.Size( 358, 80 );
            this.txtMapDescription.TabIndex = 3;
            this.txtMapDescription.Text = "[Select a map]";
            // 
            // lblMapDescription
            // 
            this.lblMapDescription.AutoSize = true;
            this.lblMapDescription.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblMapDescription.Location = new System.Drawing.Point( 13, 84 );
            this.lblMapDescription.Name = "lblMapDescription";
            this.lblMapDescription.Size = new System.Drawing.Size( 93, 20 );
            this.lblMapDescription.TabIndex = 2;
            this.lblMapDescription.Text = "Description:";
            // 
            // lblMapAuthor
            // 
            this.lblMapAuthor.AutoSize = true;
            this.lblMapAuthor.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblMapAuthor.Location = new System.Drawing.Point( 13, 52 );
            this.lblMapAuthor.Name = "lblMapAuthor";
            this.lblMapAuthor.Size = new System.Drawing.Size( 166, 20 );
            this.lblMapAuthor.TabIndex = 1;
            this.lblMapAuthor.Text = "Author: [Select a Map]";
            // 
            // lblMapName
            // 
            this.lblMapName.AutoSize = true;
            this.lblMapName.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.lblMapName.Location = new System.Drawing.Point( 13, 22 );
            this.lblMapName.Name = "lblMapName";
            this.lblMapName.Size = new System.Drawing.Size( 160, 20 );
            this.lblMapName.TabIndex = 0;
            this.lblMapName.Text = "Name: [Select a Map]";
            // 
            // gbOnlineMaps
            // 
            this.gbOnlineMaps.Controls.Add( this.btnDownloadMap );
            this.gbOnlineMaps.Controls.Add( this.liOnlineMaps );
            this.gbOnlineMaps.Location = new System.Drawing.Point( 6, 6 );
            this.gbOnlineMaps.Name = "gbOnlineMaps";
            this.gbOnlineMaps.Size = new System.Drawing.Size( 181, 471 );
            this.gbOnlineMaps.TabIndex = 1;
            this.gbOnlineMaps.TabStop = false;
            this.gbOnlineMaps.Text = "Online Maps";
            // 
            // btnDownloadMap
            // 
            this.btnDownloadMap.Enabled = false;
            this.btnDownloadMap.Location = new System.Drawing.Point( 6, 436 );
            this.btnDownloadMap.Name = "btnDownloadMap";
            this.btnDownloadMap.Size = new System.Drawing.Size( 169, 26 );
            this.btnDownloadMap.TabIndex = 2;
            this.btnDownloadMap.Text = "Download Map";
            this.btnDownloadMap.UseVisualStyleBackColor = true;
            this.btnDownloadMap.Click += new System.EventHandler( this.btnDownloadMap_Click );
            // 
            // liOnlineMaps
            // 
            this.liOnlineMaps.FormattingEnabled = true;
            this.liOnlineMaps.Location = new System.Drawing.Point( 6, 22 );
            this.liOnlineMaps.Name = "liOnlineMaps";
            this.liOnlineMaps.Size = new System.Drawing.Size( 169, 407 );
            this.liOnlineMaps.TabIndex = 3;
            this.liOnlineMaps.SelectedIndexChanged += new System.EventHandler( this.liOnlineMaps_SelectedIndexChanged );
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 627, 532 );
            this.Controls.Add( this.tabControl1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "Window";
            this.Text = "Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Window_FormClosing );
            this.Load += new System.EventHandler( this.Window_Load );
            this.gbCTFLogs.ResumeLayout( false );
            this.gbCTFLogs.PerformLayout();
            this.gbPlayerControls.ResumeLayout( false );
            this.tabControl1.ResumeLayout( false );
            this.tpMain.ResumeLayout( false );
            this.tpMain.PerformLayout();
            this.gbServerLogs.ResumeLayout( false );
            this.gbServerLogs.PerformLayout();
            this.tpCTF.ResumeLayout( false );
            this.tpCTF.PerformLayout();
            this.groupBox5.ResumeLayout( false );
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout( false );
            this.groupBox4.ResumeLayout( false );
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout( false );
            this.tpMaps.ResumeLayout( false );
            this.gbMapPreview.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).EndInit();
            this.gbMapInfo.ResumeLayout( false );
            this.gbMapInfo.PerformLayout();
            this.gbOnlineMaps.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TextBox txtServerLogs;
        private System.Windows.Forms.ListBox liClients;
        private System.Windows.Forms.GroupBox gbCTFLogs;
        private System.Windows.Forms.TextBox txtCTFLogs;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnBan;
        private System.Windows.Forms.Button btnKickban;
        private System.Windows.Forms.Button btnBanIp;
        private System.Windows.Forms.Button btnChangeRank;
        private System.Windows.Forms.GroupBox gbPlayerControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpMain;
        private System.Windows.Forms.GroupBox gbServerLogs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tpCTF;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListBox liBluePlayers;
        private System.Windows.Forms.Label lbBlueFlagStatus;
        private System.Windows.Forms.Label lbBlueScore;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox liRedPlayers;
        private System.Windows.Forms.Label lbRedFlagStatus;
        private System.Windows.Forms.Label lbRedScore;
        private System.Windows.Forms.Label lbBlueWins;
        private System.Windows.Forms.Label lbRedWins;
        private System.Windows.Forms.Label lbCurrentLevel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tpMaps;
        private System.Windows.Forms.GroupBox gbMapPreview;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbMapInfo;
        private System.Windows.Forms.TextBox txtMapDescription;
        private System.Windows.Forms.Label lblMapDescription;
        private System.Windows.Forms.Label lblMapAuthor;
        private System.Windows.Forms.Label lblMapName;
        private System.Windows.Forms.GroupBox gbOnlineMaps;
        private System.Windows.Forms.Button btnDownloadMap;
        private System.Windows.Forms.ListBox liOnlineMaps;
    }
}