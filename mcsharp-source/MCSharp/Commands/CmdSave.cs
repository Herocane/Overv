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
    public class CmdSave : Command
    {
        public override string name { get { return "save"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
        public CmdSave() { }
        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                if (message != "") { Help(p); return; }
                p.level.Save();
                p.SendMessage("Level \"" + p.level.name + "\" saved.");
            }
            else
            {
                foreach (Level l in Server.levels)
                {
                    l.Save();
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/save - Saves the level, not an actual backup.");
        }
    }
}