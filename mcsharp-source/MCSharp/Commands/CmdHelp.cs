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

namespace Overv {
	public class CmdHelp : Command {
        public override string name { get { return "help"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdHelp() {  }
		public override void Use(Player p,string message)  
        {
            string category = "";
            switch ( message.ToLower() ) {
                case "":
                    p.SendMessage( "Use &a/help Groups&S to view a list of groups." );
                    p.SendMessage( "Use &a/help All&S to view a list of &aAll&S commands." );
                    p.SendMessage( "Use &a/help Build&S to view a list of &aBuilding&S commands." );
                    p.SendMessage( "Use &a/help Mod&S to view a list of &aModeration&S commands." );
                    p.SendMessage( "Use &a/help Info&S to view a list of &aInfo&S commands." );
                    p.SendMessage( "Use &a/help Other&S to view a list of &aOther&S commands." );
                    p.SendMessage( "Use &a/help [command]&S or &a/help [block]&S for more info." );
                    break;
                case "groups":
                    foreach ( Group grp in Group.groups ) {
                        p.SendMessage( grp.color + grp.name + "&b - Draw Limit: " + grp.drawLimit + "&c - Permission: " + grp.permission.GetHashCode() );
                    }
                    break;
                case "all":                    
                    category = message;
                    message = "";
                    Command.all.commands.ForEach( delegate( Command cmd ) {
                        message += ", " + GetColor( cmd ) + cmd.name;
                    } );
                    if ( message == "" ) {
                        message = category + " commands unavailable to you.";
                    } else {
                        message = message.Remove( 0, 2 );
                    }
                    p.SendMessage( category + "&S commands: " + message );
                    break;
                case "build":
                    category = message;
                    message = "";
                    Command.all.commands.ForEach( delegate( Command cmd ) {
                        if ( cmd.type == category && p.group.canUse( cmd ) ) {
                            message += ", " + GetColor( cmd ) + cmd.name;
                        }
                    } );
                    if ( message == "" ) {
                        message = "No " + category + " commands available to you.";
                    } else {
                        message = message.Remove( 0, 2 );
                    }
                    p.SendMessage( category + "&S commands: " + message );
                    break;
                case "mod":
                    category = message;
                    message = "";
                    Command.all.commands.ForEach( delegate( Command cmd ) {
                        if ( cmd.type == category && p.group.canUse( cmd ) ) {
                            message += ", " + GetColor( cmd ) + cmd.name;
                        }
                    } );
                    if ( message == "" ) {
                        message = "No " + category + " commands available to you.";
                    } else {
                        message = message.Remove( 0, 2 );
                    }
                    p.SendMessage( category + "&S commands: " + message );
                    break;
                case "info":
                    category = message;
                    message = "";
                    Command.all.commands.ForEach( delegate( Command cmd ) {
                        if ( cmd.type == category && p.group.canUse( cmd ) ) {
                            message += ", " + GetColor( cmd ) + cmd.name;
                        }
                    } );
                    if ( message == "" ) {
                        message = "No " + category + " commands available to you.";
                    } else {
                        message = message.Remove( 0, 2 );
                    }
                    p.SendMessage( category + "&S commands: " + message );
                    break;
                case "other":
                    category = message;
                    message = "";
                    Command.all.commands.ForEach( delegate( Command cmd ) {
                        if ( cmd.type == category && p.group.canUse( cmd ) ) {
                            message += ", " + GetColor( cmd ) + cmd.name;
                        }
                    } );
                    if ( message == "" ) {
                        message = "No " + category + " commands available to you.";
                    } else {
                        message = message.Remove( 0, 2 );
                    }
                    p.SendMessage( category + "&S commands: " + message );
                    break;
                default:
                    if ( Command.all.Find( message ) != null ) {
                        Command.all.Find( message ).Help( p );
                        p.SendMessage( "Rank needed: " + GetColor( Command.all.Find( message ) ) + Group.commandPerms.Find( cmda => cmda.cmd == Command.all.Find( message ) ).perm );
                    } else if ( Block.Byte( message ) != Block.Zero ) {
                        p.SendMessage( "Block &b" + Block.Name( Block.Byte( message ) ) + "&S appears as &3" + Block.Name( Block.Convert( Block.Byte( message ) ) ) );
                    } else {
                        p.SendMessage( "Could not find specified command or block \"" + message + "\"." );
                    }
                    break;
            }
		}

        public string GetColor( Command cmd ) {
            foreach ( Group.CommandAllowance cmda in Group.commandPerms ) {
                if ( cmda.cmd.name == cmd.name ) {
                    if ( Group.Find( cmda.perm ) == null ) {
                        return Group.defaultGroup.color;
                    } else {
                        return Group.Find( cmda.perm ).color;
                    }
                }
            }
            return Group.defaultGroup.color;
        }

        public override void Help(Player p)  
        {
			p.SendMessage("/help [command] - Shows a list of commands or more detail for a specific command.");
		}
	}
}