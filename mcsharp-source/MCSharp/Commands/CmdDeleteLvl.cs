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
using System.IO;

namespace Overv
{
    class CmdDeleteLvl : Command
    {
        public override string name { get { return "deletelvl"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdDeleteLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            foreach (Level l in Server.levels)
            {
                if (l.name.ToLower() == message)
                {
                    Command.all.Find("unload").Use(p, message);
                    File.Delete("levels/" + l.name + ".lvl");
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/deletelvl - perminatly deletes a level.");
        }
    }
}
