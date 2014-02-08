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
using System.Text;

namespace Overv
{
    public class CmdPlayers : Command
    {

        public override string name { get { return "players"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdPlayers() { }

        public override void Use( Player p, string message ) {
            string playerList = "";

            Player.players.ForEach( delegate( Player pl ) {
                playerList += ", " + pl.color + pl.name + " (" + pl.team.name + ")";
            } );

            if ( playerList != "" ) {
                p.SendMessage( "Players online: " + playerList.Remove( 0, 2 ) );
            } else {
                p.SendMessage( "There are no players online! Whut?" );
            }
        }

        public override void Help( Player p ) {
            p.SendMessage( "/players - Displays a list of players currently online." );
        }
    }
}
