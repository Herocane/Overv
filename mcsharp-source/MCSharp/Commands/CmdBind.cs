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
	public class CmdBind : Command {
        public override string name { get { return "bind"; } }
        public override string type { get { return "build"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Builder; } }
		public CmdBind() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			if (message.Split(' ').Length > 2) { Help(p); return; }
			message = message.ToLower();
			int pos = message.IndexOf(' ');
			if (pos != -1) {
				byte b1 = Block.Byte(message.Substring(0,pos));
				if (b1==255) { p.SendMessage("There is no block \""+message.Substring(0,pos)+"\"."); return; }
				if (!Block.Placable(b1)) { p.SendMessage("You can't bind "+Block.Name(b1)+"."); return; }
                byte b2 = Block.Byte(message.Substring(pos + 1));
                if (b2 == 255) { p.SendMessage("There is no block \"" + message.Substring(pos + 1) + "\"."); return; }
                if (b2 == 0 || b2 == 8 || b2 == 10) { p.SendMessage("You can't bind " + Block.Name(b2) + "."); return; }
                if (Block.Placable(b2)) { p.SendMessage(Block.Name(b2) + " isn't a special block."); return; }
                if (p.bindings[b1] == b2) { p.SendMessage(Block.Name(b1) + " is already bound to " + Block.Name(b2) + "."); return; }
				p.bindings[b1] = b2;
                message = Block.Name(b1) + " bound to " + Block.Name(b2) + ".";
				for (byte i=0;i<128;++i) 
                {
                    
					byte b = i;
					if (p.bindings[i]==b2 && i!=b1 && Block.Placable(b)) {
						message += " Unbound "+Block.Name(b)+".";
						p.bindings[i] = i; break;
					}
				} p.SendMessage(message);
			} else {
				byte b = Block.Byte(message);
				if (b==255) { p.SendMessage("There is no block \""+message+"\"."); return; }
				if (!Block.Placable(b)) { p.SendMessage("You can't place "+Block.Name(b)+"."); return; }
                if (p.bindings[b] == b) { p.SendMessage(Block.Name(b) + " isn't bound."); return; }
                p.bindings[b] = b; p.SendMessage("Unbound " + Block.Name(b) + ".");
			}
		} public override void Help(Player p)  {
			p.SendMessage("/bind <block> [type] - Replaces block with type.");
		}
	}
}