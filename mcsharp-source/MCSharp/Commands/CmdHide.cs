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
	public class CmdHide : Command {
        public override string name { get { return "hide"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
		public CmdHide() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			p.hidden = !p.hidden; 
            if (p.hidden) { 
                Player.GlobalDie(p,true);
                Player.GlobalMessageOps("To Ops &f-" + p.color + p.name + "&f-&S is now &finvisible&S.");
                Player.GlobalChat(p, "&c- " + p.color + p.name + "&S disconnected.", false);
				//p.SendMessage("You're now &finvisible&S.");
			} else {
				Player.GlobalSpawn(p,p.pos[0],p.pos[1],p.pos[2],p.rot[0],p.rot[1],false);
                Player.GlobalMessageOps("To Ops &f-" + p.color + p.name + "&f-&S is now &8visible&S.");
                Player.GlobalChat(p, "&a+ " + p.color + p.name + "&S joined the game.", false);
				//p.SendMessage("You're now &8visible&S.");
			}
		} public override void Help(Player p)  {
			p.SendMessage("/hide - Makes yourself (in)visible to other players.");
		}
	}
}