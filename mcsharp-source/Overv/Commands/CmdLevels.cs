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
using System.IO;
using System.Collections.Generic;

namespace Overv {
	public class CmdLevels : Command {
        public override string name { get { return "levels"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdLevels() {  }
		public override void Use(Player p,string message)  { // TODO
			if (message != "") { Help(p); return; }
			List<string> levels = new List<string>(Server.levels.Count);
			message = Server.mainLevel.name;
            string message2 = "";
            levels.Add(Server.mainLevel.name.ToLower());
            bool Once = false;
			Server.levels.ForEach(delegate(Level level) 
            { 
                if (level != Server.mainLevel) 
                {
                    if (level.permissionvisit <= p.group.permission)
                    {
                        message += ", " + level.name;
                        levels.Add(level.name.ToLower());
                    }
                    else
                    {
                        if (!Once)
                        {
                            Once = true;
                            message2 += level.name;
                        }
                        else
                        {
                            message2 += ", " + level.name;
                        }
                    }
                } 
            });
            p.SendMessage("Loaded: &2" + message);
            p.SendMessage("Can't Goto: &c" + message2);
			message = "";
			DirectoryInfo di = new DirectoryInfo("levels/");
			FileInfo[] fi = di.GetFiles("*.lvl");
            Once = false;
            foreach (FileInfo file in fi)
            {
                if (!levels.Contains(file.Name.Replace(".lvl", "").ToLower()))
                {
                    if (!Once)
                    {
                        Once = true;
                        message += file.Name.Replace(".lvl", "");
                    }
                    else
                    {
                        message += ", " + file.Name.Replace(".lvl", "");
                    }
                }
            }
            p.SendMessage("Unloaded: &4" + message);
		} public override void Help(Player p)  {
			p.SendMessage("/levels - Lists all levels.");
		}
	}
}