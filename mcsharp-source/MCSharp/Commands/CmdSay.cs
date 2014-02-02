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
using System.Text;

namespace Overv
{
    class CmdSay : Command
    {
        public override string name { get { return "say"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdSay() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            message = "&S" + message; // defaults to yellow
            message = message.Replace("%", "&"); // Alow colors in global messages
            Player.GlobalChat(p,message,false);
            message = message.Replace("&", ""); // converts the MC color codes to IRC. Doesn't seem to work with multiple colors
            IRCBot.Say("Global: " + message);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/say - brodcasts a global message to everyone in the server.");
        }
    }
}
