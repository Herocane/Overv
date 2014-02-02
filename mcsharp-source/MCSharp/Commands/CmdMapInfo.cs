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
    public class CmdMapInfo : Command
    {
        public override string name { get { return "mapinfo"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdMapInfo() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            p.SendMessage("Currently on &b" + p.level.name + "&S X:" + p.level.width.ToString() + " Y:" + p.level.depth.ToString() + " Z:" + p.level.height.ToString());
            switch (p.level.physics)
            {
                case 0:
                    p.SendMessage("Physics is &cOFF&S.");
                    break;

                case 1:
                    p.SendMessage("Physics is &aNormal&S.");
                    break;

                case 2:
                    p.SendMessage("Physics is &aAdvanced&S.");
                    break;
            }

            p.SendMessage("Build rank = " + Level.PermissionToName(p.level.permissionbuild) + " : Visit rank = " + Level.PermissionToName(p.level.permissionvisit) + ".");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/mapinfo - Display details of the current map.");
        }
    }
}