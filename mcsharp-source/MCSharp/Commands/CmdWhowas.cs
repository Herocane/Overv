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
using System.Collections.Generic;
namespace Overv
{
    public class CmdWhowas : Command
    {
        public override string name { get { return "whowas"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdWhowas() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player pl = Player.Find(message); if (pl != null)
            { p.SendMessage(pl.color + pl.name + "&S is online, use /whois instead."); return; }
            if (!Player.left.ContainsKey(message.ToLower()))
            { p.SendMessage("No entry found for \"" + message + "\"."); return; }
            //LeftPlayer who = Player.left[message.ToLower()];
			string playerName = message.ToLower();
			string ip = Player.left[playerName];

			message = "&S" + playerName + " is " + Player.GetColor(playerName) + Player.GetGroup(playerName).name + "&S.";
            if (p.group == Group.Find("operator") || p.group == Group.Find("superOp"))
            {
                message += " IP: " + ip + ".";
                /*if (Player.GetGroup(who.name) != Group.Find("operator"))
                {
                    p.SendChat(p, message);
                    //message = "&SActions: " + who.actions.Count + ".";
                    message += " Write \"/undo " + who.name + "\" to undo actions.";
                }
                //else { message += " Actions: " + who.actions.Count + "."; }*/
            } p.SendMessage(message);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/whowas <name> - Displays information about someone who left.");
        }
    }
}