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
	public class CmdTp : Command {
        public override string name { get { return "tp"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
		public CmdTp() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			Player who = Player.Find(message);
			if (who == null) { p.SendMessage("There is no player \""+message+"\"!"); return; }
            if (p.level != who.level) { p.SendMessage("-" + who.group.color + who.name + "&S- is on &b" + who.level.name + "&S."); return; }
			unchecked { p.SendPos((byte)-1,who.pos[0],who.pos[1],who.pos[2],who.rot[0],0); }
		} public override void Help(Player p)  {
			p.SendMessage("/tp <player> - Teleports yourself to a player.");
		}
	}
}