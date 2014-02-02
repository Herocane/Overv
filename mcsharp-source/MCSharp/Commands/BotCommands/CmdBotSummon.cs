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
    public class CmdBotSummon : Command
    {
        public override string name { get { return "botsummon"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdBotSummon() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            PlayerBot who = PlayerBot.Find(message);
            if (who == null) { p.SendMessage("There is no bot " + message + "!"); return; }
            if (p.level != who.level) { p.SendMessage(who.name + " is in a different level."); return; }
            who.SetPos( p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0);
            //who.SendMessage("You were summoned by " + p.color + p.name + "&S.");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/botsummon <name> - Summons a bot to your position.");
        }
    }
}