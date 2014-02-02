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

namespace Overv {
	public class CmdSetRank : Command {
        public override string name { get { return "setrank"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
		public CmdSetRank() {  }

		public override void Use(Player p, string message)  {
            if ( message.Split( ' ' ).Length < 2 ) {
                Help( p );
                return;
            }

            string[] args = message.Split( ' ' );
            Player who = Player.Find( args[0] );
            Group newGroup = Group.Find( args[1] );
            bool isPromotion = true;

            if ( who == null ) {
                p.SendMessage( "Could not find player \"" + args[0] + "\"!" );
                return;
            } else if ( newGroup == null ) {
                p.SendMessage( "Could not find group \"" + args[1] + "\"!" );
                return;
            } else if ( who == p ) {
                p.SendMessage( "You can't set your own rank!" );
                return;
            } else if ( who.group.permission >= p.group.permission ) {
                p.SendMessage( "Player must have a lower rank than your own!" );
                return;
            } else if ( newGroup.permission >= p.group.permission ) {
                p.SendMessage( "You can't set a players rank equal or higher to yours!" );
                return;
            }

            if ( newGroup.permission < who.group.permission ) {
                isPromotion = false;
            }

            who.group.players.Remove( who.name );
            who.group.players.Save( who.group.name + ".txt" );
            newGroup.players.Add( who.name );
            newGroup.players.Save( newGroup.name + ".txt" );
            who.group = newGroup;

            if ( isPromotion ) {
                Player.GlobalMessage( who.color + who.name + "&S was promoted to " + newGroup.color + newGroup.name + "&S!" );
            } else {
                Player.GlobalMessage( who.color + who.name + "&S was demoted to " + newGroup.color + newGroup.name + "&S!" );
            }

            Player.GlobalDie( who, false );
            Player.GlobalSpawn( who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false );
		} 
        
        public override void Help(Player p)  {
			p.SendMessage("/setrank [player] [rank] - sets a player's rank.");
		}
	}
}