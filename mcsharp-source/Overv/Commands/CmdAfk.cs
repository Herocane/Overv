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
    class CmdAfk : Command
    {
        public override string name { get { return "afk"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdAfk() { }

        public override void Use(Player p, string message)
        {
            if (message != "list")
            {
                if (Server.afkset.Contains(p.name))
                {
                    Server.afkset.Remove(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&S- is no longer AFK");
                    IRCBot.Say(p.name + " is no longer AFK");
                }
                else
                {
                    Server.afkset.Add(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&S- is AFK " + message);
                    IRCBot.Say(p.name + " is AFK " + message);
                }
            }
            else
            {
                foreach (string s in Server.afkset)
                {
                    p.SendMessage(s);
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/afk <reason> - mark yourself as AFK. Use again to mark yourself as back");
        }
    }
}
