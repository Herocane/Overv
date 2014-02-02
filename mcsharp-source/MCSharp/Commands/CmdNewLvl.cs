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
    class CmdNewLvl : Command
    {
        public override string name { get { return "newlvl"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdNewLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            string[] parameters = message.Split(' '); // Grab the parameters from the player's message
            if (parameters.Length == 5) // make sure there are 5 params
            {
                switch(parameters[4])
                {
                    case "flat":
                    case "pixel":
                    case "island":
                    case "mountains":
                    case "ocean":
                    case "forest":
                    case "empty":
                        break;
                    default:
                        p.SendMessage("Valid types: island, mountains, forest, ocean, flat, pixel, empty"); return;
                }

                string name = parameters[0];
                // create a new level...
                try
                {
                    Level lvl = new Level(name,
                                          Convert.ToUInt16(parameters[1]),
                                          Convert.ToUInt16(parameters[2]),
                                          Convert.ToUInt16(parameters[3]),
                                          parameters[4]);
                    lvl.author = p.name;
                    lvl.SaveProperties(); //... and save it.
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
                Player.GlobalMessage("&3" + name + " was created."); // The player needs some form of confirmation.
            }
            else
                p.SendMessage("Not enough parameters! <name> <x> <y> <z> <type>"); // Yell at the player for failing
        }
        public override void Help(Player p)
        {
            p.SendMessage("/newlvl - creates a new level.");
            p.SendMessage("/newlvl mapname 128 64 128 type");
        }
    }
}
