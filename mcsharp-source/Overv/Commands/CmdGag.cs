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
    public class CmdGag : Command {
        public override string name { get { return "gag"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdGag() { }

        public override void Use( Player p, string message ) {
            Player who = Player.Find( message.Split( ' ' )[0].Trim() );
            if ( who == null ) {
                p.SendMessage( "Player " + message + " was not found." );
                return;
            }

            who.isGagged = !who.isGagged;

            if ( message.Contains( "silent" ) ) {
                Player.GlobalMessage( "&3(Someone) " + ( ( who.isGagged ) ? "&Sgagged " : "&Sun-gagged " ) + who.color + who.name + "&S." );
            } else {
                Player.GlobalMessage( p.color + p.name + " " + ( ( who.isGagged ) ? "&Sgagged " : "&Sun-gagged " ) + who.color + who.name + "&S." );
            }
        }

        public override void Help( Player p ) {
            p.SendMessage( "/gag [player] <silent> - gags or un-gags a player." );
            p.SendMessage( "Add \"silent\" to the message to make it silent." );
        }
    }
}