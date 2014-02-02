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
using System.IO;

namespace Overv
{
    public class Group {
        public static List<Group> groups = new List<Group>();
        public static List<CommandAllowance> commandPerms = new List<CommandAllowance>();
        public static Group defaultGroup;
        public static string groupsFile = "groups.properties";

        public string name;
        public string color;
        public bool canChat;
        public bool canBuild;
        public bool canBreakBedrock;
        public int drawLimit;
        public LevelPermission permission;
        public CommandList commands;
        public PlayerList players;
        public bool canUse(Command cmd) { return commands.Contains(cmd); }

        public Group() { }

        public Group( string newName, string newColor, bool newCanChat, int newDrawLimit, LevelPermission newPermission, bool newCanBreakBedrock, bool newCanBuild ) {
            name = newName;
            color = newColor;
            canChat = newCanChat;
            canBuild = newCanBuild;
            drawLimit = newDrawLimit;
            permission = newPermission;
            canBreakBedrock = newCanBreakBedrock;
            players = PlayerList.Load( name + ".txt" );
            commands = new CommandList();
        }

        public static void Load() {
            if ( File.Exists( groupsFile ) ) {
                Group newGroup = new Group();
                int index = 0;

                foreach ( string line in File.ReadAllLines( groupsFile ) ) {
                    if ( !string.IsNullOrEmpty( line ) && !line.StartsWith( "#" ) ) {
                        string key = line.Split( '=' )[0].Trim();
                        string value = line.Split( '=' )[1].Trim();

                        switch ( key.ToLower() ) {
                            case "name":
                                index++;
                                newGroup.name = value;
                                break;
                            case "color":
                                index++;
                                newGroup.color = value;
                                break;
                            case "canchat":
                                index++;
                                newGroup.canChat = bool.Parse( value );
                                break;
                            case "canbuild":
                                index++;
                                newGroup.canBuild = bool.Parse( value );
                                break;
                            case "drawlimit":
                                index++;
                                newGroup.drawLimit = int.Parse( value );
                                break;
                            case "canbreakbedrock":
                                index++;
                                newGroup.canBreakBedrock = bool.Parse( value );
                                break;
                            case "permission":
                                index++;
                                newGroup.permission = (LevelPermission)int.Parse( value );
                                if ( newGroup.permission >= LevelPermission.Null ) {
                                    newGroup.permission = (LevelPermission)149;
                                    Server.s.Log( "Group permission can't be more than 149! Lowering to 149..." );
                                }
                                break;
                        }

                        if ( index == 7 ) {
                            groups.Add( new Group( newGroup.name, newGroup.color, newGroup.canChat, newGroup.drawLimit, newGroup.permission, newGroup.canBreakBedrock, newGroup.canBuild ) );
                            index = 0;
                        }
                    }
                }
            }

            if ( groups.Find( grp => grp.permission == LevelPermission.Griefer ) == null ) { groups.Add( new Group( "Griefer", "&8", true, 0, LevelPermission.Griefer, false, false ) ); }
            if ( groups.Find( grp => grp.permission == LevelPermission.Guest ) == null ) { groups.Add( new Group( "Guest", "&7", true, 0, LevelPermission.Guest, false, true ) ); }
            if ( groups.Find( grp => grp.permission == LevelPermission.Builder ) == null ) { groups.Add( new Group( "Builder", "&2", true, 500, LevelPermission.Builder, false, true ) ); }
            if ( groups.Find( grp => grp.permission == LevelPermission.AdvBuilder ) == null ) { groups.Add( new Group( "AdvBuilder", "&3", true, 1500, LevelPermission.AdvBuilder, false, true ) ); }
            if ( groups.Find( grp => grp.permission == LevelPermission.Operator ) == null ) { groups.Add( new Group( "Operator", "&c", true, 5000, LevelPermission.Operator, true, true ) ); }
            if ( groups.Find( grp => grp.permission == LevelPermission.Admin ) == null ) { groups.Add( new Group( "SuperOp", "&e", true, 15000, LevelPermission.Admin, true, true ) ); }

            defaultGroup = Group.Find( LevelPermission.Guest );
            Save();
        }

        public static void Save() {
            StreamWriter sw = new StreamWriter( File.Create( groupsFile ) );
            foreach ( Group gr in groups ) {
                sw.WriteLine( "Name = " + gr.name );
                sw.WriteLine( "Color = " + gr.color );
                sw.WriteLine( "CanChat = " + gr.canChat.ToString() );
                sw.WriteLine( "CanBuild = " + gr.canBuild.ToString() );
                sw.WriteLine( "DrawLimit = " + gr.drawLimit );
                sw.WriteLine( "CanBreakBedrock = " + gr.canBreakBedrock.ToString() );
                sw.WriteLine( "Permission = " + gr.permission.GetHashCode() );
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        void FillCommands() {
            foreach ( CommandAllowance cmda in commandPerms ) {
                if ( cmda.perm <= permission ) {
                    commands.Add( cmda.cmd );
                }
            }
        }

        public static void LoadCommands() {
            if ( File.Exists( "command.properties" ) ) {
                foreach ( string line in File.ReadAllLines( "command.properties" ) ) {
                    Command cmd = Command.all.Find( line.Split( ':' )[0] );
                    LevelPermission perm = (LevelPermission)int.Parse( line.Split( ':' )[1] );

                    if ( cmd != null ) {
                        commandPerms.Add( new CommandAllowance( cmd, perm ) );
                    } else {
                        Server.s.Log( "Invalid command allowances: " + line + "..." );
                    }
                }
            }

            foreach ( Command cmd in Command.all.commands ) {
                if ( commandPerms.Find( cmda => cmda.cmd == cmd ) == null ) {
                    commandPerms.Add( new CommandAllowance( cmd, cmd.defaultPerm ) );
                    Server.s.Log( "Added CommandAllowance: " + cmd.name + " = " + cmd.defaultPerm.GetHashCode() );
                }
            }

            foreach ( Group gr in groups ) {
                gr.FillCommands();
            }

            Server.s.Log( "LOADED: command.properties..." );
            SaveCommands();
        }

        public static void SaveCommands() {
            StreamWriter sw = new StreamWriter( File.Create( "command.properties" ) );
            foreach ( CommandAllowance cmda in commandPerms ) {
                sw.WriteLine( cmda.cmd.name + ":" + cmda.perm.GetHashCode() );
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public class CommandAllowance {
            public Command cmd;
            public LevelPermission perm;

            public CommandAllowance( Command ncmd, LevelPermission nperm ) {
                cmd = ncmd;
                perm = nperm;
            }
        }

        public static bool Exists( string name ) {
            foreach ( Group gr in groups ) {
                if ( gr.name.ToLower() == name.ToLower() ) {
                    return true;
                }
            }
            return false;
        }

        public static Group FindPlayer( string name ) {
            foreach ( Group gr in groups ) {
                if ( gr.players.Contains( name ) ) {
                    return gr;
                }
            }
            return defaultGroup;
        }

        public static Group Find( string name ) {
            foreach ( Group gr in groups ) {
                if ( gr.name.ToLower() == name.ToLower() ) {
                    return gr;
                }
            }
            return null;
        }

        public static Group Find( LevelPermission perm ) {
            foreach ( Group gr in groups ) {
                if ( gr.permission == perm ) {
                    return gr;
                }
            }
            return null;
        }
    }
}