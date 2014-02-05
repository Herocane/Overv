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
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;

using MonoTorrent.Client;

namespace Overv {
    public class Server {
        public delegate void LogHandler( string message );
        public delegate void HeartBeatHandler();
        public delegate void MessageEventHandler( string message );
        public delegate void PlayerListHandler( List<Player> playerList );
        public delegate void VoidHandler();

        public event LogHandler OnLog;
        public event LogHandler OnCTFLog;
        public event HeartBeatHandler HeartBeatFail;
        public event MessageEventHandler OnURLChange;
        public event PlayerListHandler OnPlayerListChange;
        public event VoidHandler OnSettingsUpdate;


        public static string Version { get { return "211"; } }

        static Socket listen;
        static System.Diagnostics.Process process;
        static System.Timers.Timer updateTimer = new System.Timers.Timer( 100 );
        static System.Timers.Timer messageTimer = new System.Timers.Timer( 60000 * 5 );   //Every 5 mins

        static Thread physThread;

        public static PlayerList banned;
        public static PlayerList bannedIP;
        public static MapGenerator MapGen;

        public static Thread checkPosThread;

        public static PerformanceCounter PCCounter;
        public static PerformanceCounter ProcessCounter;


        public static PlayerList ircControllers;

        public static Level mainLevel;
        public static List<Level> levels;
        public static List<string> afkset = new List<string>();
        public static List<string> messages = new List<string>();

        #region Server Settings
        public const byte version = 7;
        public static string salt = "";

        public static string name = "(Capture The Flag) Server";
        public static string motd = "Welcome to my server! &c-hax";
        public static byte players = 12;
        public static byte maps = 5;
        public static int port = 25565;
        public static bool pub = true;
        public static bool verify = false;
        public static bool worldChat = true;
        public static bool guestGoto = false;

        public static string systemColor = "&e";

        public static string level = "ctf_arena";
        public static string errlog = "error.log";

        public static bool console = false;
        public static bool reportBack = true;

        public static bool irc = false;
        public static int ircPort = 6667;
        public static string ircNick = "changethis";
        public static string ircServer = "irc.esper.net";
        public static string ircChannel = "#changethis";
        public static bool ircIdentify = false;
        public static string ircPassword = "";

        public static bool allowflying = true;
        public static bool allownoclip = true;
        public static bool allowspeeding = true;
        public static bool allowrespawning = true;
        public static bool allowthirdperson = true;
        public static short jumpheight = 40;

        public static bool texturepacksenabled = false;
        public static string textureurl = "";
        public static byte sideblock = Block.water;
        public static byte edgeblock = Block.blackrock;
        public static short sidelevel = 32;

        public static bool antiTunnel = true;
        public static byte maxDepth = 4;
        public static int Overload = 1500;
        public static int backupInterval = 150;
        #endregion

        public static MainLoop ml;
        public static Server s;
        public Server() {
            ml = new MainLoop( "server" );
            Server.s = this;

        }
        public void Start() {
            Log( "Starting server..." );
            Properties.Load();

            ml.Queue( delegate {
                levels = new List<Level>( Server.maps );
                MapGen = new MapGenerator();

                Random random = new Random();

                if ( File.Exists( "levels/" + Server.level + ".lvl" ) ) {
                    mainLevel = Level.Load( Server.level );
                    if ( mainLevel == null ) {
                        if ( File.Exists( "levels/" + Server.level + ".lvl.backup" ) ) {
                            Log( "Atempting to load backup." );
                            File.Copy( "levels/" + Server.level + ".lvl.backup", "levels/" + Server.level + ".lvl", true );
                            mainLevel = Level.Load( Server.level );
                            if ( mainLevel == null ) {
                                Log( "BACKUP FAILED!" );
                                Console.ReadKey(); return;
                            }
                        } else {
                            Log( "BACKUP NOT FOUND!" );
                            Console.ReadKey(); return;
                        }

                    }
                } else {
                    mainLevel = new Level( Server.level, 128, 64, 128, "flat" );

                    mainLevel.permissionvisit = LevelPermission.Guest;
                    mainLevel.permissionbuild = LevelPermission.Guest;
                    mainLevel.Save();
                }
                levels.Add( mainLevel );
            } );

            ml.Queue( delegate {
                banned = PlayerList.Load( "banned.txt" );
                bannedIP = PlayerList.Load( "banned-ip.txt" );
                ircControllers = PlayerList.Load( "../IRC_Controllers.txt" );

                Command.Load();
                Group.Load();
                Group.LoadCommands();
                CTF.redTeam = new Team( "&c", Block.red );
                CTF.blueTeam = new Team( "&9", Block.deepblue );
                CTF.Setup(Server.mainLevel, true);
            } );

            ml.Queue( delegate {
                if ( File.Exists( "autoload.txt" ) ) {
                    try {
                        string[] lines = File.ReadAllLines( "autoload.txt" );
                        foreach ( string line in lines ) {
                            //int temp = 0;
                            string _line = line.Trim();
                            try {

                                if ( _line == "" ) { continue; }
                                if ( _line[0] == '#' ) { continue; }
                                int index = _line.IndexOf( "=" );

                                string key = line.Split( '=' )[0].Trim();
                                string value;
                                try {
                                    value = line.Split( '=' )[1].Trim();
                                } catch {
                                    value = "0";
                                }

                                if ( !key.Equals( "main" ) ) {
                                    Command.all.Find( "load" ).Use( null, key + " " + value );
                                } else {
                                    try {
                                        int temp = int.Parse( value );
                                        if ( temp >= 0 && temp <= 2 ) {
                                            mainLevel.physics = temp;
                                        }
                                    } catch {
                                        Server.s.Log( "Physics variable invalid..." );
                                    }
                                }


                            } catch {
                                Server.s.Log( _line + " failed..." );
                            }
                        }
                    } catch {
                        Server.s.Log( "autoload.txt error..." );
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                } else {
                    Log( "autoload.txt does not exist..." );
                }
            } );

            ml.Queue( delegate {
                if ( Setup() ) {
                    s.Log( "Created listening socket on port " + port + "..." );
                } else {
                    s.Log( "Could not create socket connection, shutting down..." );
                    return;
                }
            } );

            ml.Queue( delegate {
                updateTimer.Elapsed += delegate {
                    Player.GlobalUpdate();
                };

                updateTimer.Start();


            } );
            // Heartbeat code here:

            Heartbeat.Init();

            // END Heartbeat code

            PCCounter = new PerformanceCounter( "Processor", "% Processor Time", "_Total" );
            ProcessCounter = new PerformanceCounter( "Process", "% Processor Time", Process.GetCurrentProcess().ProcessName );
            Server.PCCounter.BeginInit();
            ProcessCounter.BeginInit();
            PCCounter.NextValue();
            ProcessCounter.NextValue();
            physThread = new Thread( new ThreadStart( Physics ) );
            physThread.Start();


            ml.Queue( delegate {
                messageTimer.Elapsed += delegate {
                    RandomMessage();
                };
                messageTimer.Start();
                process = System.Diagnostics.Process.GetCurrentProcess();

                if ( File.Exists( "messages.txt" ) ) {
                    StreamReader r = File.OpenText( "messages.txt" );
                    while ( !r.EndOfStream )
                        messages.Add( r.ReadLine() );
                } else
                    File.Create( "messages.txt" ).Close();

                if ( Server.irc )
                    new IRCBot();

                new AutoSaver( Server.backupInterval );
            } );

            ml.Queue( delegate {
                checkPosThread = new Thread( new ThreadStart( delegate {
                    while ( true ) {
                        Player.players.ForEach( delegate( Player p ) {
                            p.CheckPosition();
                        } );
                        Thread.Sleep( 250 );
                    }
                } ) );
                checkPosThread.Start();
            } );

            ml.Queue( delegate {
                s.Log( "Finished setting up server..." );
            } );
        }

        static bool Setup() {
            try {
                IPEndPoint endpoint = new IPEndPoint( IPAddress.Any, Server.port );
                listen = new Socket( endpoint.Address.AddressFamily,
                                    SocketType.Stream, ProtocolType.Tcp );
                listen.Bind( endpoint );
                listen.Listen( (int)SocketOptionName.MaxConnections );

                listen.BeginAccept( new AsyncCallback( Accept ), null );
                return true;
            } catch ( SocketException e ) { ErrorLog( e ); return false; } catch ( Exception e ) { ErrorLog( e ); return false; }
        }

        static void Accept( IAsyncResult result ) {
            // found information: http://www.codeguru.com/csharp/csharp/cs_network/sockets/article.php/c7695
            // -Descention
            try {
                new Player( listen.EndAccept( result ) );
                listen.BeginAccept( new AsyncCallback( Accept ), null );
            } catch ( SocketException e ) {
                //s.Close();
                ErrorLog( e );
            } catch ( Exception e ) {
                //s.Close(); 
                ErrorLog( e );
            }
        }

        public static void Exit() {
            Player.players.ForEach( delegate( Player p ) { p.Kick( "Server shutdown." ); } );
            Player.connections.ForEach( delegate( Player p ) { p.Kick( "Server shutdown." ); } );

            Logger.Dispose();

            if ( physThread != null )
                physThread.Abort();
            if ( process != null ) {
                ErrorLog( "Process Terminated" );
                process.Kill();
            }

        }

        public void PlayerListUpdate() {
            if ( Server.s.OnPlayerListChange != null ) Server.s.OnPlayerListChange( Player.players );
        }

        public void FailBeat() {
            if ( HeartBeatFail != null ) HeartBeatFail();
        }

        public void UpdateUrl( string url ) {
            if ( OnURLChange != null ) OnURLChange( url );
        }

        public void Log( string message ) {
            if ( OnLog != null ) OnLog( DateTime.Now.ToString( "(HH:mm:ss) " ) + message );
            Logger.Write( DateTime.Now.ToString( "(HH:mm:ss) " ) + message + Environment.NewLine );
        }

        public void CTFLog( string message ) {
            if ( OnCTFLog != null ) OnCTFLog( DateTime.Now.ToString( "(HH:mm:ss) " ) + message );
        }

        public static void ErrorLog( string message ) {
            if ( Server.errlog == "" ) { Console.WriteLine( DateTime.Now.ToString( "(HH:mm:ss) " ) + "ERROR!" ); } else {
                Console.WriteLine( DateTime.Now.ToString( "(HH:mm:ss) " ) + "ERROR! See \"" + Server.errlog + "\" for more information." );
                StreamWriter sw = File.AppendText( Server.errlog );
                sw.WriteLine( DateTime.Now.ToString( "(HH:mm:ss)" ) );
                sw.WriteLine( message ); sw.Close();
            }
        }

        public static void ErrorLog( Exception ex ) {
            Logger.WriteError( ex );
        }

        public static void ParseInput()        //Handle console commands
        {
            string cmd;
            string msg;
            while ( true ) {
                string input = Console.ReadLine();
                if ( input == null )
                    continue;
                cmd = input.Split( ' ' )[0];
                if ( input.Split( ' ' ).Length > 1 )
                    msg = input.Substring( input.IndexOf( ' ' ) ).Trim();
                else
                    msg = "";
                try {
                    switch ( cmd ) {
                        case "kick":
                            Command.all.Find( "kick" ).Use( null, msg ); break;
                        case "ban":
                            Command.all.Find( "ban" ).Use( null, msg ); break;
                        case "banip":
                            Command.all.Find( "banip" ).Use( null, msg ); break;
                        case "resetbot":
                            Command.all.Find( "resetbot" ).Use( null, msg ); break;
                        case "save":
                            Command.all.Find( "save" ).Use( null, msg ); break;
                        case "say":
                            if ( !msg.Equals( "" ) ) {
                                if ( Properties.ValidString( msg, "![]&:.,{}~-+()?_/\\@%$ " ) ) {
                                    Player.GlobalMessage( msg );
                                } else {
                                    Console.WriteLine( "bad char in say" );
                                }
                            }
                            break;
                        case "guest":
                            Command.all.Find( "guest" ).Use( null, msg ); break;
                        case "builder":
                            Command.all.Find( "builder" ).Use( null, msg ); break;
                        case "help":
                            Console.WriteLine( "ban, banip, builder, guest, kick, resetbot, save, say" );
                            break;
                        default:
                            Console.WriteLine( "No such command!" ); break;
                    }
                } catch ( Exception e ) { ErrorLog( e ); }
                //Thread.Sleep(10);
            }
        }

        public static void Physics() {
            int wait = 250;
            while ( true ) {
                try {
                    if ( wait > 0 ) {
                        Thread.Sleep( wait );
                    }
                    DateTime Start = DateTime.Now;
                    levels.ForEach( delegate( Level L )    //update every level
                    {
                        L.CalcPhysics();
                    } );
                    TimeSpan Took = DateTime.Now - Start;
                    wait = (int)250 - (int)Took.TotalMilliseconds;
                    if ( wait < -Server.Overload ) {
                        levels.ForEach( delegate( Level L )    //update every level
                        {
                            try {
                                L.physics = 0;
                                L.ClearPhysics();
                            } catch {

                            }
                        } );
                        Server.s.Log( "!PHYSICS SHUTDOWN!" );
                        Player.GlobalMessage( "!PHYSICS SHUTDOWN!" );
                        wait = 250;
                    } else if ( wait < (int)( -Server.Overload * 0.75f ) ) {
                        Server.s.Log( "!PHYSICS WARNING!" );
                    }
                } catch {
                    Server.s.Log( "GAH! PHYSICS EXPLODING!" );
                    wait = 250;
                }

            }
        }

        public static void RandomMessage() {
            if ( Player.number != 0 && messages.Count > 0 )
                Player.GlobalMessage( messages[new Random().Next( 0, messages.Count )] );
        }

        internal void SettingsUpdate() {
            if ( OnSettingsUpdate != null ) OnSettingsUpdate();
        }
    }
}