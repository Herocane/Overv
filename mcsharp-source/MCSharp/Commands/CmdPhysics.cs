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
    public class CmdPhysics : Command
    {
        public override string name { get { return "physics"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdPhysics() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { if (p != null) { Help(p); } return; }
            try
            {
                int temp = int.Parse(message);
                if (temp >= 0 && temp <= 2)
                {
                    p.level.physics = temp;
                    switch (temp)
                    {
                        case 0:
                            p.level.ClearPhysics();
                            Player.GlobalMessageLevel(p.level, "Physics is now &cOFF&S on &b" + p.level.name + "&S.");
                            Server.s.Log("Physics is now OFF on " + p.level.name + ".");
                            IRCBot.Say("Physics is now OFF on " + p.level.name + ".");
                            break;

                        case 1:
                            Player.GlobalMessageLevel(p.level, "Physics is now &aNormal&S on &b" + p.level.name + "&S.");
                            Server.s.Log("Physics is now ON on " + p.level.name + ".");
                            IRCBot.Say("Physics is now ON on " + p.level.name + ".");
                            break;

                        case 2:
                            Player.GlobalMessageLevel(p.level,"Physics is now &aAdvanced&S on &b" + p.level.name + "&S.");
                            Server.s.Log("Physics is now ADVANCED on " + p.level.name + ".");
                            IRCBot.Say("Physics is now ADVANCED on " + p.level.name + ".");
                            break;
                    }
                    
                }
                else
                {
                    if (p != null) { p.SendMessage("Not a valid setting"); }
                }
            }
            catch
            {
                if (p != null) { p.SendMessage("INVALID INPUT"); }
            }

        }

        public override void Help(Player p)
        {
            p.SendMessage("/physics <0/1/2> - Set the levels physics, 0-Off 1-On 2-Advanced");
        }
    }
}