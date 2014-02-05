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

namespace Overv {
	public class CmdGoto : Command {
        public override string name { get { return "goto"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdGoto() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			
			foreach (Level level in Server.levels) {
				if (level.name.ToLower() == message.ToLower()) 
                {
					if (p.level == level) { p.SendMessage("You are already in \""+level.name+"\"."); return; }
                    if (p.group.permission < level.permissionvisit) { p.SendMessage("Your not allowed to goto " + level.name + "."); return; }
                    p.Loading = true;

					foreach (Player pl in Player.players) 
					{
                        if ( p.level == pl.level && p != pl ) {
                            p.SendDie( pl.playerID );
                        }
                        if ( p.level == pl.level ) {
                            p.SendDeletePlayerName( pl.playerID );
                        }
					}

                    p.BlockAction = 0;
                    p.painting = false;
                    Player.GlobalDie(p,true);
					p.level = level; 
					p.SendMotd(); 
					p.SendMap();
					ushort x = (ushort)((0.5+level.spawnx)*32);
					ushort y = (ushort)((1+level.spawny)*32);
					ushort z = (ushort)((0.5+level.spawnz)*32);
					if (!p.hidden)
					{
						Player.GlobalSpawn(p, x, y, z, level.rotx, level.roty, true);
					}
					else unchecked
						{
							p.SendPos((byte)-1, x, y, z, level.rotx, level.roty);
						}
					foreach (Player pl in Player.players) 
					{
						if (pl.level == p.level && p != pl && !pl.hidden)
						{ 
							p.SendSpawn(pl.playerID,pl.color+pl.name,pl.pos[0],pl.pos[1],pl.pos[2],pl.rot[0],pl.rot[1]); 
						}
                        if ( pl.level == p.level ) {
                            p.SendAddPlayerName( pl.playerID, pl.color + pl.name, pl.name, pl.group.color + pl.group.name, (byte)pl.group.permission );
                        }
					}

                    p.Loading = false;
                    try {
                        p.SendMessage( "This map's author: &b" + p.level.author );
                        int percentage = ( p.level.likes * 100 ) / ( p.level.likes + p.level.dislikes );
                        if ( percentage >= 50 ) {
                            p.SendMessage( "This map has a upvote percentage of &a" + percentage + "%&S!" );
                        } else {
                            p.SendMessage( "This map has a upvote percentage of &c" + percentage + "%&S!" );
                        }
                    } catch { }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
					return;
				}
			}

            if ( Level.Load( message.ToLower() ) != null ) {
                Level foundLevel = Level.Load( message.ToLower() );
                Server.levels.Add( foundLevel );

                Use( p, message );
            } else {
                p.SendMessage( "Level \"" + message + "\" does not exist." );
            }
		} public override void Help(Player p)  {
			p.SendMessage("/goto <mapname> - Teleports yourself to a different level.");
		}
	}
}