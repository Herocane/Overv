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
    public class CmdKick : Command {
        public override string name { get { return "kick"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
        public CmdKick() { }

        public override void Use( Player p, string message ) {
            if ( p != null ) {
                if ( message == "" ) { Help( p ); return; }
                string who = message;
                int index = message.IndexOf( ' ' );
                if ( index != -1 ) {
                    who = message.Substring( 0, index );
                    message = message.Substring( index + 1 );
                } if ( Player.Exists( who ) ) {
                    Player kick = Player.Find( who );
                    if ( kick == p ) { p.SendMessage( "You can't kick yourself!" ); return; }
                    if ( kick.group == Group.Find( "operator" ) ) {
                        if ( p.group != Group.Find( "superOp" ) )   //Allow Super ops to kick Op's if required Ie AFK ones.
                        {
                            p.SendMessage( "You can't kick an operator!" ); return;
                        }
                    }
                    if ( kick.group == Group.Find( "superOp" ) ) { p.SendMessage( "You can't kick a Super Op!" ); return; }
                    if ( index == -1 ) { kick.Kick( "You were kicked by " + p.name + "!" ); IRCBot.Say( who + " was kicked by " + p.name ); } else { kick.Kick( message ); IRCBot.Say( who + " was kicked by " + p.name + "(" + message + ")" ); }
                } else { p.SendMessage( "There is no player \"" + who + "\"!" ); }
            } else {
                if ( message == "" ) { return; }
                string who = message;
                int index = message.IndexOf( ' ' );
                if ( index != -1 ) {
                    who = message.Substring( 0, index );
                    message = message.Substring( index + 1 );
                }
                if ( Player.Exists( who ) ) {
                    Player kick = Player.Find( who );
                    if ( kick.group == Group.Find( "operator" ) ) { return; }
                    if ( kick.group == Group.Find( "superOp" ) ) { return; }
                    if ( index == -1 ) { kick.Kick( "You were kicked by [console]!" ); IRCBot.Say( who + " was kicked by [console]" ); } else { kick.Kick( message ); IRCBot.Say( who + " was kicked (" + message + ")" ); }
                }
            }
        }

        public override void Help( Player p ) {
            p.SendMessage( "/kick <player> [message] - Kicks a player." );
        }
    }
}