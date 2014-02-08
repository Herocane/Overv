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
    public class CmdBan : Command
    {
        public override string name { get { return "ban"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdBan() { }
        public override void Use(Player p, string message)
        {
            if (p != null)
            {

                if (message == "") { Help(p); return; }
                if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
                if (Server.banned.Contains(message)) { p.SendMessage(message + " is already banned."); return; }
                Player who = Player.Find(message);

                if ( who == null ) {
                    Player.GlobalMessage( message + " &f(offline)&8 was banned." );
                } else {
                    Player.GlobalChat( who, who.color + who.name + "&8 was banned.", false );
                    who.group = Group.Find( "banned" ); who.color = who.group.color; Player.GlobalDie( who, false );
                    Player.GlobalSpawn( who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false );
                }

                Server.banned.Add(message); 
                Server.banned.Save("banned.txt", false); 
                IRCBot.Say(message + " was banned by " + p.name);
                Server.s.Log("BANNED: " + message.ToLower());
            }
            else
            {
                if (message == "") {return; }
                if (!Player.ValidName(message)) { return; }
                if (Server.banned.Contains(message)) { return; }
                Player who = Player.Find(message);

                if ( who == null ) { Player.GlobalMessage( message + " &f(offline)&8 was banned." ); } else {
                    Player.GlobalChat( who, who.color + who.name + "&8 was banned.", false );
                    who.group = Group.Find( "banned" ); who.color = who.group.color; Player.GlobalDie( who, false );
                    Player.GlobalSpawn( who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false );
                }

                Server.banned.Add(message); 
                Server.banned.Save("banned.txt", false); 
                IRCBot.Say(message + " was banned by [console]");
                Server.s.Log("BANNED: " + message.ToLower());
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/ban <player> - Bans a player without kicking him.");
            p.SendMessage("Add # before name to stealth ban.");
        }
    }
}