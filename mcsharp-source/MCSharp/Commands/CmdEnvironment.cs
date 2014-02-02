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
    class CmdEnvironment : Command
    {
        public override string name { get { return "env"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdEnvironment() { }

        public override void Use(Player p, string message)
        {
            string type = message.Split( ' ' )[0];
            string red = message.Split( ' ' )[1];
            string green = message.Split( ' ' )[2];
            string blue = message.Split( ' ' )[3];
            byte envType = 0;

            switch ( type.ToLower() ) {
                case "sky":
                    envType = 0;
                    break;
                case "clouds":
                    envType = 1;
                    break;
                case "fog":
                    envType = 2;
                    break;
                case "ambient":
                case "ambientlight":
                    envType = 3;
                    break;
                case "diffuse":
                case "diffuselight":
                    envType = 4;
                    break;
            }

            List<Player> thisLevel = Player.players.FindAll( ply => ply.level == p.level );
            foreach ( Player pl in thisLevel ) {
                pl.SendEnvSetColor( envType, short.Parse( red ), short.Parse( green ), short.Parse( blue ) );
                pl.SendMessage( "Set &c" + type + " color&S to RGB(" + red + ", " + green + ", " + blue + ")." );
            }
        }

        public override void Help(Player p)
        {
            p.SendMessage("/env [type] [r] [g] [b] - Changes the env colors.");
            p.SendMessage( "&c/env only works with ClassiCube clients!" );
            p.SendMessage( "Available types: sky, clouds, fog, amblientlight (ambient), diffuselight (diffuse)." );
        }
    }
}
