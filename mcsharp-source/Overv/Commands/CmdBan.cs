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

namespace Overv
{
    public class CmdBan : Command
    {
        public override string name { get { return "ban"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdBan() { }
        public override void Use(Player p, string message) {
            bool silent = false;
            if ( message.Contains( "silent" ) ) {
                silent = true;
            }

            Player who = Player.Find( message.Split( ' ' )[0].Trim() );
            if ( who == p || who.group.permission <= p.group.permission ) {
                return;
            }

            Server.banned.Add( message.Split( ' ' )[0].Trim() );
            Server.banned.Save( "banned.txt" );

            if ( p != null ) {
                if ( who != null ) {
                    if ( silent ) {
                        Player.GlobalMessage( "&3(Someone) &Sbanned " + who.color + who.name );
                    } else {
                        Player.GlobalMessage( p.color + p.name + " &Sbanned " + who.color + who.name );
                    }
                } else {
                    if ( silent ) {
                        Player.GlobalMessage( "&3(Someone) &Sbanned " + message.Split( ' ' )[0].Trim() + " &f(offline)" );
                    } else {
                        Player.GlobalMessage( p.color + p.name + " &Sbanned " + message.Split( ' ' )[0].Trim() + " &f(offline)" );
                    }
                }
            } else {
                if ( who != null ) {
                    if ( silent ) {
                        Player.GlobalMessage( "&3(Someone) &Sbanned " + who.color + who.name );
                    } else {
                        Player.GlobalMessage( "&8Console &Sbanned " + who.color + who.name );
                    }
                } else {
                    if ( silent ) {
                        Player.GlobalMessage( "&3(Someone) &Sbanned " + message.Split( ' ' )[0].Trim() + " &f(offline)" );
                    } else {
                        Player.GlobalMessage( "&8Console &Sbanned " + message.Split( ' ' )[0].Trim() + " &f(offline)" );
                    }
                }
            }

            if ( who != null ) {
                who.Kick( "You've been banned!" );
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/ban <player> - Bans a player without kicking him.");
            p.SendMessage("Add # before name to stealth ban.");
        }
    }
}