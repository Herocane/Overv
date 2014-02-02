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
	public class CmdSpawn : Command {
        public override string name { get { return "spawn"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdSpawn() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
            if ( p.team != null ) {
                p.SendMessage( "&f- &cUse of this command is prohibited during game time." );
            }
			ushort x = (ushort)((0.5+p.level.spawnx)*32);
            ushort y = (ushort)((1 + p.level.spawny) * 32);
            ushort z = (ushort)((0.5 + p.level.spawnz) * 32);
			unchecked { p.SendPos((byte)-1,x,y,z,
                                    p.level.rotx,
                                    p.level.roty);
            }
		} public override void Help(Player p)  {
			p.SendMessage("/spawn - Teleports yourself to the spawn location.");
		}
	}
}