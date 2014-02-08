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
	public class CmdMove : Command {
        public override string name { get { return "move"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
		public CmdMove() {  }

		public override void Use(Player p,string message)  {
            string[] values = message.Split( ' ' );
            if ( values.Length != 4 && p != null ) {
                Help( p );
                return;
            }

            Player targetPlayer = Player.Find( values[0].Trim() );

            if ( targetPlayer != null ) {
                if ( p != null && targetPlayer.level != p.level ) {
                    p.SendMessage( "Player is on a different map!" );
                    return;
                }

                ushort x = ushort.Parse( values[1] );
                ushort y = ushort.Parse( values[2] );
                ushort z = ushort.Parse( values[3] ); 
                x *= 32; x += 16;
                y *= 32; y += 32;
                z *= 32; z += 16;

                unchecked {
                    targetPlayer.SendPos( (byte)-1, x, y, z, targetPlayer.rot[0], targetPlayer.rot[1] );
                }
            }
		} 
        
        public override void Help(Player p)  {
			p.SendMessage("/move [player] [x] [y] [z] - Moves player to a specified position.");
		}
	}
}