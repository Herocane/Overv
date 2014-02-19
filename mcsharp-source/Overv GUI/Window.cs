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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.IO.Compression;

using Overv;

namespace Overv.Gui
{
    public partial class Window : Form
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
        // for cross thread use
        delegate void StringCallback(string s);
        delegate void PlayerListCallback(List<Player> players);
        delegate void VoidDelegate();

        internal static Server s;
        
        bool shuttingDown = false;
        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            this.Text = "<server name here>";
            
            s = new Server();
            s.OnLog += WriteLine;
            s.OnCTFLog += WriteCTFLine;
            s.HeartBeatFail += HeartBeatFail;
            s.OnURLChange += UpdateUrl;
            s.OnPlayerUpdate += UpdatePlayerBalloon;
            s.OnPlayerListChange += UpdateClientList;
            s.OnSettingsUpdate += SettingsUpdate;
            s.Start();

            System.Timers.Timer statsTimer = new System.Timers.Timer( 2000 );
            statsTimer.Elapsed += delegate {
                UpdateStats();
            };
            statsTimer.Start();

            UpdateMaps();
        }

        static List<MapInfo> availableMaps = new List<MapInfo>();

        struct MapInfo {
            public string name;
            public string author;
            public string description;
            public string image;

            public MapInfo( string nname, string nauthor, string ndescription, string nimage ) {
                name = nname;
                author = nauthor;
                description = ndescription;
                image = nimage;
            }
        }

        void UpdateMaps() {
            try {
                WebClient wc = new WebClient();
                wc.DownloadFile( "http://mcderp.x10.mx/downloads/availablemaps.txt", "text/maps.txt" );
                foreach ( string line in File.ReadAllLines( "text/maps.txt" ) ) {
                    availableMaps.Add( new MapInfo( line.Split( ':' )[0].Trim(), line.Split( ':' )[1].Trim(), line.Split( ':' )[2].Trim(), line.Split( ':' )[3].Trim() ) );
                }
                wc.Dispose();
            } catch(WebException we) {
                MessageBox.Show( "Unable to download maplist! Details: " + we.ToString() );
            }

            foreach ( MapInfo mi in availableMaps ) {
                liOnlineMaps.Items.Add( mi.name );
            }
        }

        void UpdateStats() {
            if ( this.InvokeRequired ) {
                VoidDelegate d = new VoidDelegate( UpdateStats );
                this.Invoke( d );
            } else {
                try {
                    lbCurrentLevel.Text = "Currently playing on: " + CTF.currLevel.name;
                    lbRedScore.Text = "Score: " + CTF.redTeam.points;
                    lbBlueScore.Text = "Score: " + CTF.blueTeam.points;
                    lbRedWins.Text = "Wins: " + CTF.redWins;
                    lbBlueWins.Text = "Wins: " + CTF.blueWins;
                    if ( CTF.redTeam.flagIsHome ) {
                        lbRedFlagStatus.Text = "Flag: is home.";
                    } else if ( CTF.redTeam.hasFlag != null ) {
                        lbRedFlagStatus.Text = "Flag: " + CTF.redTeam.hasFlag.name + " has it!";
                    } else {
                        lbRedFlagStatus.Text = "Flag: dropped.";
                    }
                    if ( CTF.blueTeam.flagIsHome ) {
                        lbBlueFlagStatus.Text = "Flag: is home.";
                    } else if ( CTF.blueTeam.hasFlag != null ) {
                        lbBlueFlagStatus.Text = "Flag: " + CTF.blueTeam.hasFlag.name + " has it!";
                    } else {
                        lbBlueFlagStatus.Text = "Flag: dropped.";
                    }
                    liRedPlayers.Items.Clear();
                    CTF.redTeam.players.ForEach( delegate( Player p ) {
                        liRedPlayers.Items.Add( p.name );
                    } );
                    liBluePlayers.Items.Clear();
                    CTF.blueTeam.players.ForEach( delegate( Player p ) {
                        liBluePlayers.Items.Add( p.name );
                    } );
                } catch { 
                    // ctf is probably disabled.                 
                }
            }
        }

        void SettingsUpdate() {
            if ( shuttingDown ) return;
            if ( txtServerLogs.InvokeRequired ) {
                VoidDelegate d = new VoidDelegate( SettingsUpdate );
                this.Invoke( d );
            } else {
                this.Text = "Capture The Flag: " + Server.name;
            }
        }

        void HeartBeatFail()
        {
            WriteLine("Recent Heartbeat Failed");
        }

        delegate void LogDelegate(string message);

        /// <summary>
        /// Does the same as Console.Write() only in the form
        /// </summary>
        /// <param name="s">The string to write</param>
        public void Write(string s)
        {
            if (shuttingDown) return;
            if (txtServerLogs.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(Write);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                txtServerLogs.AppendText(s);
            }
        }
        /// <summary>
        /// Does the same as Console.WriteLine() only in the form
        /// </summary>
        /// <param name="s">The line to write</param>
        public void WriteLine(string s)
        {
            if (shuttingDown) return;
            if (this.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(WriteLine);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                txtServerLogs.AppendText(s + "\r\n");
            }
            
        }

        /// <summary>
        /// CTF LOGZZZ
        /// </summary>
        /// <param name="s"></param>
        public void WriteCTFLine( string s ) {
            if ( shuttingDown ) return;
            if ( this.InvokeRequired ) {
                LogDelegate d = new LogDelegate( WriteCTFLine );
                this.Invoke( d, new object[] { s } );
            } else {
                txtCTFLogs.AppendText( s + "\r\n" );
            }
        }

        /// <summary>
        /// Updates the list of client names in the window
        /// </summary>
        /// <param name="players">The list of players to add</param>
        public void UpdateClientList(List<Player> players)
        {
            if (this.InvokeRequired)
            {
                PlayerListCallback d = new PlayerListCallback(UpdateClientList);
                this.Invoke(d, new object[] { players });
            }
            else
            {
                liClients.Items.Clear();
                Player.players.ForEach(delegate(Player p) { liClients.Items.Add(p.name); });
            }
        }

        /// <summary>
        /// Pops up a little tray balloon.
        /// </summary>
        /// <param name="hasLeft"></param>
        public void UpdatePlayerBalloon( Player p, bool hasLeft ) {
            NotifyIcon notify = new NotifyIcon();
            notify.Icon = this.Icon;
            notify.Visible = true;
            notify.BalloonTipIcon = ToolTipIcon.Info;
            notify.BalloonTipTitle = p.name + ((hasLeft == true) ? " left the game." : " joined the game.");
            notify.BalloonTipText = "There are now " + ((hasLeft == true) ? (Player.players.Count - 1) : Player.players.Count) + " players online.";
            notify.ShowBalloonTip( 3000 );
        }

        /// <summary>
        /// Places the server's URL at the top of the window
        /// </summary>
        /// <param name="s">The URL to display</param>
        public void UpdateUrl(string s)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(UpdateUrl);
                this.Invoke(d, new object[] { s });
            }
            else
                txtUrl.Text = s;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            shuttingDown = true;
            Logger.Dispose();
            Server.Exit();
        }

        private void btnKick_Click(object sender, EventArgs e)
        {
            if (liClients.SelectedIndex >= 0)
            {
                Player p = Player.Find(liClients.SelectedItem.ToString());
                if (p != null)
                {
                    p.Kick("You were kicked by [console]!"); 
                    //IRCBot.Say(p.name + " was kicked by [console]!");
                    UpdateClientList(Player.players);
                }
            }
            else
                MessageBox.Show("You need to select someone");
        }

        private void btnBan_Click( object sender, EventArgs e ) {
            if ( liClients.SelectedIndex >= 0 ) {
                Server.banned.Add( liClients.SelectedItem.ToString() );
                Server.banned.Save( "banned.txt" );
            }
        }

        private void btnKickban_Click( object sender, EventArgs e ) {
            btnKick_Click( sender, e );
            btnBan_Click( sender, e );
        }

        private void btnBanIp_Click( object sender, EventArgs e ) {
            if ( liClients.SelectedIndex >= 0 ) {
                string message = liClients.SelectedItem.ToString();
                Player who = null;
                who = Player.Find( message );
                if ( who != null )
                    message = who.ip;

                if ( message.Equals( "127.0.0.1" ) ) { MessageBox.Show( "You can't ip-ban the server!" ); return; }
                if ( !regex.IsMatch( message ) ) { MessageBox.Show( "Not a valid ip!" ); return; }
                if ( Server.bannedIP.Contains( message ) ) { MessageBox.Show( message + " is already ip-banned." ); return; }
                Player.GlobalMessage( message + " got &8ip-banned&S!" );
                //IRCBot.Say("IP-BANNED: " + message.ToLower() + " by console");
                Server.bannedIP.Add( message ); Server.bannedIP.Save( "banned-ip.txt", false );
                s.Log( "IP-BANNED: " + message.ToLower() );
            } else
                MessageBox.Show( "You need to select someone" );
        }

        private void btnChangeRank_Click( object sender, EventArgs e ) {
            MessageBox.Show( "Not implemented yet" );
        }

        private void txtInput_KeyDown( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == Keys.Enter ) {
                Player.GlobalChat( null, txtChat.Text );
                Server.s.Log( "<Console> " + txtChat.Text );
                txtChat.Clear();
            }
        }

        bool isExpanded = false;

        private void button2_Click( object sender, EventArgs e ) {
            if ( isExpanded ) {
                gbCTFLogs.Width = 267;
                txtCTFLogs.Width = 229;
                button2.Text = ">";
                button2.Location = new Point( 241, 19 );
                gbPlayerControls.Visible = true;
            } else {
                gbCTFLogs.Width = gbServerLogs.Width;
                txtCTFLogs.Width = 408;
                button2.Text = "<";
                button2.Location = new Point( 420, 19 );
                gbPlayerControls.Visible = false;
            }

            isExpanded = !isExpanded;
        }

        private void liOnlineMaps_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( liOnlineMaps.SelectedIndex != -1 ) {
                btnDownloadMap.Enabled = true;

                MapInfo match = new MapInfo("N/a", "N/a", "N/a", "N/a");

                foreach ( MapInfo mi in availableMaps ) {
                    if ( mi.name == liOnlineMaps.SelectedItem.ToString() ) {
                        match = mi;
                    }
                }

                lblMapName.Text = "Name: " + match.name;
                lblMapAuthor.Text = "Author: " + match.author;
                txtMapDescription.Text = match.description;

                try {
                    pictureBox1.ImageLocation = "http://mcderp.x10.mx/downloads/" + match.image;
                } catch {

                }
            }
        }

        private void btnDownloadMap_Click( object sender, EventArgs e ) {
            foreach ( MapInfo mi in availableMaps ) {
                if ( mi.name == liOnlineMaps.SelectedItem.ToString() ) {
                    try {
                        WebClient wc = new WebClient();
                        wc.DownloadFile( "http://mcderp.x10.mx/downloads/" + mi.name + ".lvl", "levels/" + mi.name + ".lvl" );
                        wc.DownloadFile( "http://mcderp.x10.mx/downloads/" + mi.name + ".properties", "levels/" + mi.name + ".properties" );
                        wc.Dispose();

                        Server.s.Log( "Downloaded \"" + mi.name + "\" from http://mcderp.x10.mx/..." );
                    } catch(WebException we) {
                        MessageBox.Show( "An error occured whilst attempting to download your map. Details: " + we.ToString() );
                    }
                }
            }
        }
    }
}
