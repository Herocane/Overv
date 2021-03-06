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
using System.Threading;

namespace Overv {
    class CmdRedTeam : Command {
        public override string name { get { return "red"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
        public CmdRedTeam() { }

        public override void Use( Player p, string message ) {
            switch ( message.ToLower() ) {
                case "":
                    if ( p.team == CTF.redTeam ) {
                        p.SendMessage( "You're already on that team!" );
                        return;
                    }

                    if ( p.team == CTF.blueTeam ) {
                        if ( CTF.redTeam.players.Count <= CTF.blueTeam.players.Count ) {
                            CTF.blueTeam.DelPlayer( p );
                            CTF.redTeam.AddPlayer( p );
                        } else {
                            p.SendMessage( "Joining the red team would cause imbalance!" );
                        }
                        return;
                    }

                    CTF.redTeam.AddPlayer( p );
                    break;
                case "spawn":
                    if ( p.group.permission < LevelPermission.Operator ) {
                        p.SendMessage( "Only ops+ can set the team spawn." );
                        break;
                    }
                    ushort x = (ushort)( p.pos[0] / 32 );
                    ushort y = (ushort)( p.pos[1] / 32 );
                    ushort z = (ushort)( p.pos[2] / 32 );
                    byte rx = p.rot[0];
                    byte ry = p.rot[1];
                    if ( CTF.currLevel == p.level ) {
                        CTF.redTeam.spawn = new ushort[3] { x, y, z };
                        CTF.redTeam.spawnrot = new byte[2] { rx, ry };
                    }
                    p.level.redSpawn = new ushort[3] { x, y, z };
                    p.level.redRotation = new byte[2] { rx, ry };
                    p.level.SaveProperties();

                    p.SendMessage( "Red spawn set!" );
                    break;
                case "flag":
                    if ( p.group.permission < LevelPermission.Operator ) {
                        p.SendMessage( "Only ops+ can set the team flag." );
                        break;
                    }
                    p.Blockchange += Blockchange;
                    p.SendMessage( "Place a block where the flag should be." );
                    break;
            }
        }

        public void Blockchange( Player p, ushort x, ushort y, ushort z, byte type ) {
            byte b = p.level.GetBlock( x, y, z );
            p.SendBlockchange( x, y, z, b );
            p.ClearBlockchange();

            if ( CTF.currLevel == p.level ) {
                CTF.redTeam.flagBase = new ushort[3] { x, y, z };
            }
            p.level.redFlag = new ushort[3] { x, y, z };
            p.level.SaveProperties();

            p.SendMessage( "Red flag set!" );
        }

        public override void Help( Player p ) {
            p.SendMessage( "/red - joins the red team." );
            p.SendMessage( "&cThese require Op+:" );
            p.SendMessage( "/red spawn - sets the red teams spawn location for that map." );
            p.SendMessage( "/red flag - sets the red teams flag location for that map." );
        }
    }
}
