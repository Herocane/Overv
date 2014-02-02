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

namespace Overv
{
    public class CmdBotRemove : Command
    {
        public override string name { get { return "botremove"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdBotRemove() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            PlayerBot who = PlayerBot.Find(message);
            if (who == null) { p.SendMessage("There is no bot " + who + "!"); return; }
            if (p.level != who.level) { p.SendMessage(who.name + " is in a different level."); return; }
            who.GlobalDie();
            //who.SendMessage("You were summoned by " + p.color + p.name + "&S.");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/botremove <name> - Remove a bot on the same level as you");
        }
    }
}