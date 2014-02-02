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

namespace Overv {
    public class CmdPortal : Command {
        public override string name { get { return "portal"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
        public CmdPortal() { }

        public override void Use( Player p, string message ) {
            if ( message == "" ) {
                cpos.type = Block.portal_blue;
            } else {
                if ( !message.Contains( "portal_" ) ) {
                    cpos.type = Block.Byte( "portal_" + message.Trim() );
                } else {
                    cpos.type = Block.Byte( message.Trim() );
                }

                if ( cpos.type == 0 ) {
                    p.SendMessage( "Invalid type: " + message );
                }
            }

            p.SendMessage( "Place a block for the &aentry." );
            p.Blockchange += Entry;
        }

        CatchPos cpos;

        public void Entry( Player p, ushort x, ushort y, ushort z, byte type ) {
            byte b = p.level.GetTile( x, y, z );
            p.SendBlockchange( x, y, z, b );
            p.ClearBlockchange();

            p.level.Blockchange( x, y, z, cpos.type );

            cpos.x = x;
            cpos.y = y;
            cpos.z = z;
            cpos.lvl = p.level;

            p.SendMessage( "Place a block for the &cexit." );
            p.Blockchange += Exit;
        }

        public void Exit( Player p, ushort x, ushort y, ushort z, byte type ) {
            byte b = p.level.GetTile( x, y, z );
            p.SendBlockchange( x, y, z, b );
            p.ClearBlockchange();

            Portal newPortal = new Portal( cpos.lvl.name, p.level.name, cpos.x, cpos.y, cpos.z, x, y, z );
            PortalDB.portals.ForEach( delegate( Portal po ) {
                if ( po.entryLevel == newPortal.entryLevel && po.entry == newPortal.entry ) {
                    PortalDB.portals.Remove( po );
                    p.SendMessage( "Overidden portal." );
                }
            } );
            PortalDB.portals.Add( newPortal );
            PortalDB.Save();

            p.SendMessage( "Portal was created!" );
        }

        public override void Help( Player p ) {
            p.SendMessage( "/portal [type] - Creates a portal." );
            p.SendMessage( "Types: air, blue, water, lava." );
        }

        struct CatchPos {
            public byte type;
            public Level lvl;
            public ushort x, y, z;
        }
    }
}