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
using System.Text.RegularExpressions;

namespace Overv
{
    public class CmdUnbanip : Command
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
        public override string name { get { return "unbanip"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Admin; } }
        public CmdUnbanip() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!regex.IsMatch(message)) { p.SendMessage("Not a valid ip!"); return; }
            if (p.ip == message) { p.SendMessage("You shouldn't be able to use this command..."); return; }
            if (!Server.bannedIP.Contains(message)) { p.SendMessage(message + " doesn't seem to be banned..."); return; }
            Player.GlobalMessage(message + " got &8unip-banned&S!");
            Server.bannedIP.Remove(message); Server.bannedIP.Save("banned-ip.txt", false);
            Server.s.Log("IP-UNBANNED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/unbanip <ip> - Un-bans an ip.");
        }
    }
}