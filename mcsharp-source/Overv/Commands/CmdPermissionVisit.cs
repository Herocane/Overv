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
    public class CmdPermissionVisit : Command
    {
        public override string name { get { return "pervisit"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdPermissionVisit() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }
                p.level.permissionvisit = Perm;
                Server.s.Log(p.level.name + " visit permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "visit permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }

                foreach (Level level in Server.levels)
                {
                    if (level.name.ToLower() == t.ToLower())
                    {
                        level.permissionvisit = Perm;
                        Server.s.Log(level.name + " visit permission changed to " + s + ".");
                        Player.GlobalMessageLevel(level, "visit permission changed to " + s + ".");
                        if (p.level != level) { p.SendMessage("visit permission changed to " + s + " on " + level.name + "."); }
                        return;
                    }
                }
                p.SendMessage("There is no level \"" + s + "\" loaded.");
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/PerVisit <rank> - Sets visiting permission for a map.");
        }
    }
}