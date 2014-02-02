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
	public class CmdInfo : Command {
        public override string name { get { return "info"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdInfo() {  }

        public override void Use( Player p, string message ) {
            p.SendMessage( "=======  &aServer Info&S  ========" );
            p.SendMessage( "Running on &cOverv&S by Marvy." );
            p.SendMessage( "Score Limit: &a" + CTF.scoreLimit );
            p.SendMessage( "&cRed Wins: &f" + CTF.redWins + " | &9Blue Wins: &f" + CTF.blueWins );
            p.SendMessage( "==========================" );
        }

        public override void Help( Player p ) {
			p.SendMessage("/info - Displays the server information.");
		}
	}
}
