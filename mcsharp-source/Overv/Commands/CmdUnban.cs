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

namespace Overv
{
    public class CmdUnban : Command
    {
        public override string name { get { return "unban"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdUnban() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (!Server.banned.Contains(message)) { p.SendMessage(message + " isn't banned."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &8(banned)&S is now " + Group.defaultGroup.color + Group.defaultGroup.name + "&S!"); }
            else
            {
                Player.GlobalChat(who, who.color + who.name + "&S is now " + Group.defaultGroup.color + Group.defaultGroup.name + "&S!", false);
                who.group = Group.defaultGroup; who.color = who.group.color; Player.GlobalDie(who, false);
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.banned.Remove(message); Server.banned.Save("banned.txt", false);
            Server.s.Log("UNBANNED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/unban <player> - Unbans a player.");
        }
    }
}