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
    public class CmdBanip : Command
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
        public override string name { get { return "banip"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Admin; } }
        public CmdBanip() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { if (p != null)Help(p); return; }
            Player who = null;
            who = Player.Find(message);
            if (who != null) { message = who.ip; }
            if (message.Equals("127.0.0.1")) { if (p != null) { p.SendMessage("You can't ip-ban the server!"); } return; }
            if (!regex.IsMatch(message)) { if (p != null)p.SendMessage("Not a valid ip!"); return; }
            if (p != null) { if (p.ip == message) { p.SendMessage("You can't ip-ban yourself.!"); return; } }
            if (Server.bannedIP.Contains(message)) { if (p != null)p.SendMessage(message + " is already ip-banned."); return; }
            Player.GlobalMessage(message + " got &8ip-banned&S!");
            if (p != null)
            { IRCBot.Say("IP-BANNED: " + message.ToLower() + " by " + p.name); }
            else
            { IRCBot.Say("IP-BANNED: " + message.ToLower() + " by console"); }
            Server.bannedIP.Add(message); Server.bannedIP.Save("banned-ip.txt", false);
            Server.s.Log("IP-BANNED: " + message.ToLower());

            foreach (Player pl in Player.players)
            {
                if (message.Equals(pl.ip)) { pl.Kick("Kicked by ipban"); }       //Kicks anyone off with matching ip for convinience
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/banip <ip/name> - Bans an ip, can also use the name of an online player.");
            p.SendMessage(" -Kicks players with matching ip as well.");
        }
    }
}