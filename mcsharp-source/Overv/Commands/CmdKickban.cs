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
    public class CmdKickban : Command
    {
        public override string name { get { return "kickban"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdKickban() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            string who = message;
            int index = message.IndexOf(' ');
            string kickmessage = "";
            if (index != -1)
            {
                who = message.Substring(0, index);
                kickmessage = message.Substring(index + 1);
            } Player kick = Player.Find(who);
            if (kick != null)
            {
                if (kick == p) { p.SendMessage("You can't kickban yourself!"); return; }
                if (kick.group == Group.Find("operator")) { p.SendMessage("You can't kickban an operator!"); return; }
                if (kick.group == Group.Find("superop")) { p.SendMessage("You can't kickban a Super Op!"); return; }
                if (Server.banned.Contains(kick.name)) { Command.all.Find("kick").Use(p, message); return; }
                if (index == -1) { kick.Kick("You were kickbanned by " + p.name + "!"); }
                else { kick.Kick(kickmessage); } Server.banned.Add(kick.name);
                Server.banned.Save("banned.txt", false);
                Server.s.Log("BANNED: " + message.ToLower());
                IRCBot.Say(kick.name + " was banned by " + p.name);
            }
            else { Command.all.Find("ban").Use(p, who); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/kickban <player> [message] - Kicks and bans a player with an optional message.");
        }
    }
}