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
using System.IO;

namespace Overv
{
    public static class Properties {
        public static void Load() {
            GenerateSalt();

            if ( File.Exists( "server.properties" ) ) {
                string[] lines = File.ReadAllLines( "server.properties" );
                foreach ( string line in lines ) {
                    if ( line != "" && line[0] != '#' ) {
                        string key = line.Split( '=' )[0].Trim();
                        string value = line.Split( '=' )[1].Trim();

                        switch ( key.ToLower() ) {
                            case "name":
                                if ( ValidString( value, "'![]:.,{}~-+()?_/\\ " ) ) {
                                    Server.name = value;
                                } else {
                                    Server.s.Log( "name invalid! setting to default." );
                                }
                                break;
                            case "motd":
                                if ( ValidString( value, "'![]&:.,{}~-+()?_/\\ " ) ) {
                                    Server.motd = value;
                                } else {
                                    Server.s.Log( "motd invalid! setting to default." );
                                }
                                break;
                            case "port":
                                try {
                                    Server.port = Convert.ToInt32( value );
                                } catch {
                                    Server.s.Log( "port invalid! setting to default." );
                                }
                                break;
                            case "verify-names":
                                Server.verify = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "public":
                                Server.pub = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "world-chat":
                                Server.worldChat = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "load-on-goto":
                                Server.guestGoto = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "max-players":
                                try {
                                    if ( Convert.ToByte( value ) > 64 ) {
                                        value = "64";
                                        Server.s.Log( "Max players has been lowered to 64." );
                                    } else if ( Convert.ToByte( value ) < 1 ) {
                                        value = "1";
                                        Server.s.Log( "Max players has been increased to 1." );
                                    }
                                    Server.players = Convert.ToByte( value );
                                } catch {
                                    Server.s.Log( "max-players invalid! setting to default." );
                                }
                                break;
                            case "max-maps":
                                try {
                                    if ( Convert.ToByte( value ) > 20 ) {
                                        value = "20";
                                        Server.s.Log( "Max maps has been lowered to 20." );
                                    } else if ( Convert.ToByte( value ) < 1 ) {
                                        value = "1";
                                        Server.s.Log( "Max maps has been increased to 1." );
                                    }
                                    Server.maps = Convert.ToByte( value );
                                } catch {
                                    Server.s.Log( "max-maps invalid! setting to default." );
                                }
                                break;
                            case "irc-enabled":
                                Server.irc = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "irc-server":
                                Server.ircServer = value;
                                break;
                            case "irc-nick":
                                Server.ircNick = value;
                                break;
                            case "irc-channel":
                                Server.ircChannel = value;
                                break;
                            case "irc-port":
                                try {
                                    Server.ircPort = Convert.ToInt32( value );
                                } catch {
                                    Server.s.Log( "irc-port invalid! setting to default." );
                                }
                                break;
                            case "irc-identify":
                                try {
                                    Server.ircIdentify = Convert.ToBoolean( value );
                                } catch {
                                    Server.s.Log( "irc-identify boolean value invalid! Setting to the default of: " + Server.ircIdentify + "." );
                                }
                                break;
                            case "irc-password":
                                Server.ircPassword = value;
                                break;
                            case "anti-tunneling":
                                Server.antiTunnel = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "max-depth":
                                try {
                                    Server.maxDepth = Convert.ToByte( value );
                                } catch {
                                    Server.s.Log( "maxDepth invalid! setting to default." );
                                }
                                break;
                            case "physics-overload":
                                try {
                                    if ( Convert.ToInt16( value ) > 5000 ) {
                                        value = "4000";
                                        Server.s.Log( "Max overload is 5000." );
                                    } else if ( Convert.ToInt16( value ) < 500 ) {
                                        value = "500";
                                        Server.s.Log( "Min overload is 500" );
                                    }
                                    Server.Overload = Convert.ToInt16( value );
                                } catch {
                                    Server.s.Log( "Overload invalid! setting to default." );
                                }
                                break;
                            case "report-back":
                                Server.reportBack = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "backup-time":
                                if ( Convert.ToInt32( value ) > 1 ) {
                                    Server.backupInterval = Convert.ToInt32( value );
                                }
                                break;
                            case "console-only":
                                Server.console = ( value.ToLower() == "true" ) ? true : false;
                                break;
                            case "score-limit":
                                CTF.scoreLimit = int.Parse( value );
                                break;
                            case "return-time":
                                CTF.returnTime = int.Parse( value );
                                break;
                            case "vote-time":
                                CTF.voteTime = int.Parse( value );
                                break;
                            case "drown-time":
                                CTF.drownTime = int.Parse( value );
                                break;
                            case "mine-activation-time":
                                CTF.mineActivationTime = int.Parse( value );
                                break;
                            case "take-flag-reward":
                                CTF.takeFlagReward = int.Parse( value );
                                break;
                            case "capture-flag-reward":
                                CTF.captureFlagReward = int.Parse( value );
                                break;
                            case "return-flag-reward":
                                CTF.returnFlagReward = int.Parse( value );
                                break;
                            case "kill-player-reward":
                                CTF.killPlayerReward = int.Parse( value );
                                break;
                            case "mine-blast-radius":
                                CTF.mineBlastRadius = int.Parse( value );
                                break;
                            case "tnt-blast-radius":
                                CTF.tntBlastRadius = int.Parse( value );
                                break;
                            case "mine-destroy-blocks":
                                CTF.mineDestroyBlocks = bool.Parse( value );
                                break;
                            case "tnt-destroy-blocks":
                                CTF.tntDestroyBlocks = bool.Parse( value );
                                break;
                            case "allow-ophax":
                                CTF.allowOpHax = bool.Parse( value );
                                break;
                        }
                    }
                }
                Server.s.Log( "LOADED: server.properties..." );
                Server.s.SettingsUpdate();
            }
            Save();
        }

        public static void GenerateSalt() {
            string rndchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            for ( int i = 0; i < 16; ++i ) {
                Server.salt += rndchars[rnd.Next( rndchars.Length )];
            }
        }

        public static bool ValidString( string str, string allowed ) {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890" + allowed;
            foreach ( char ch in str ) {
                if ( allowedchars.IndexOf( ch ) == -1 ) {
                    return false;
                }
            } return true;
        }

        static void Save() {
            try {
                StreamWriter sw = new StreamWriter( File.Create( "server.properties" ) );
                sw.WriteLine( "# Edit the settings below to modify how your server operates." );
                sw.WriteLine();
                sw.WriteLine( "# Server options:" );
                sw.WriteLine( "Name = " + Server.name );
                sw.WriteLine( "MOTD = " + Server.motd );
                sw.WriteLine( "Port = " + Server.port.ToString() );
                sw.WriteLine( "Console-Only = " + Server.console.ToString().ToLower() );
                sw.WriteLine( "Verify-Names = " + Server.verify.ToString().ToLower() );
                sw.WriteLine( "Public = " + Server.pub.ToString().ToLower() );
                sw.WriteLine( "Max-Players = " + Server.players.ToString() );
                sw.WriteLine( "Max-Maps = " + Server.maps.ToString() );
                sw.WriteLine( "World-Chat = " + Server.worldChat.ToString().ToLower() );
                sw.WriteLine( "Load-On-Goto = " + Server.guestGoto.ToString().ToLower() );
                sw.WriteLine();
                sw.WriteLine( "# CTF options:" );
                sw.WriteLine( "Score-Limit = " + CTF.scoreLimit );
                sw.WriteLine( "Return-Time = " + CTF.returnTime );
                sw.WriteLine( "Vote-Time = " + CTF.voteTime );
                sw.WriteLine( "Drown-Time = " + CTF.drownTime );
                sw.WriteLine( "Mine-Activation-Time = " + CTF.mineActivationTime );
                sw.WriteLine( "Take-Flag-Reward = " + CTF.takeFlagReward );
                sw.WriteLine( "Capture-Flag-Reward = " + CTF.captureFlagReward );
                sw.WriteLine( "Return-Flag-Reward = " + CTF.returnFlagReward );
                sw.WriteLine( "Kill-Player-Reward = " + CTF.killPlayerReward );
                sw.WriteLine( "Mine-Blast-Radius = " + CTF.mineBlastRadius );
                sw.WriteLine( "Tnt-Blast-Radius = " + CTF.tntBlastRadius );
                sw.WriteLine( "Mine-Destroy-Blocks = " + CTF.mineDestroyBlocks.ToString() );
                sw.WriteLine( "Tnt-Destroy-Blocks = " + CTF.tntDestroyBlocks.ToString() );
                sw.WriteLine( "Allow-Ophax = " + CTF.allowOpHax.ToString() );
                sw.WriteLine();
                sw.WriteLine( "# IRC options:" );
                sw.WriteLine( "IRC-Enabled = " + Server.irc.ToString() );
                sw.WriteLine( "IRC-Nick = " + Server.ircNick );
                sw.WriteLine( "IRC-Server = " + Server.ircServer );
                sw.WriteLine( "IRC-Channel = " + Server.ircChannel );
                sw.WriteLine( "IRC-Port = " + Server.ircPort );
                sw.WriteLine( "IRC-Identify = " + Server.ircIdentify );
                sw.WriteLine( "IRC-Password = " + Server.ircPassword );
                sw.WriteLine();
                sw.WriteLine( "# Hacking options:" );
                sw.WriteLine( "Allow-Flying = " + Server.allowflying.ToString() );
                sw.WriteLine( "Allow-NoClip = " + Server.allownoclip.ToString() );
                sw.WriteLine( "Allow-Speeding = " + Server.allowspeeding.ToString() );
                sw.WriteLine( "Allow-Respawning = " + Server.allowrespawning.ToString() );
                sw.WriteLine( "Allow-Thirdperson = " + Server.allowthirdperson.ToString() );
                sw.WriteLine( "Jump-Height = " + Server.jumpheight );
                sw.WriteLine();
                sw.WriteLine( "# Environment options:" );
                sw.WriteLine( "Texture-Pack-Enabled = " + Server.texturepacksenabled.ToString() );
                sw.WriteLine( "Teture-URL = " + Server.textureurl );
                sw.WriteLine( "Map-Side-Block = " + Block.Name( Server.sideblock ) );
                sw.WriteLine( "Map-Edge-Block = " + Block.Name( Server.edgeblock ) );
                sw.WriteLine( "Side-Height = " + Server.sidelevel );
                sw.WriteLine();
                sw.WriteLine( "# Other options:" );
                sw.WriteLine( "Anti-Tunnels = " + Server.antiTunnel.ToString().ToLower() );
                sw.WriteLine( "Max-Depth = " + Server.maxDepth.ToString() );
                sw.WriteLine( "Physics-Overload = " + Server.Overload.ToString() );
                sw.WriteLine( "System-Color = " + Server.systemColor );
                sw.WriteLine();
                sw.WriteLine( "# Backup options:" );
                sw.WriteLine( "Backup-Time = 150" );
                sw.WriteLine();
                sw.WriteLine( "# Error options:" );
                sw.WriteLine( "Report-Back = " + Server.reportBack.ToString().ToLower() );
                sw.Flush();
                sw.Close();
                sw.Dispose();
            } catch {
                Server.s.Log( "SAVE FAILED! server.properties" );
            }
        }
    }
}
