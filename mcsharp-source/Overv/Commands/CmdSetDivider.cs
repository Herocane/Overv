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
    class CmdSetDivider : Command {
        public override string name { get { return "setdivider"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
        public CmdSetDivider() { }

        public override void Use( Player p, string message ) {
            p.SendMessage( "&f- &cMap divider is always along the X axis, bear this in mind when making your map!" );
            p.SendMessage( "Place a block to determine the map divider." );
            p.Blockchange += Blockchange;
        }

        public void Blockchange( Player p, ushort x, ushort y, ushort z, byte type ) {
            byte b = p.level.GetBlock( x, y, z );
            p.SendBlockchange( x, y, z, b );
            p.ClearBlockchange();

            p.level.divider = x;
            p.level.SaveProperties();

            p.SendMessage( "Map divider set!" );

            Thread dividerSelection = new Thread( new ThreadStart( delegate {
                p.SendMakeSelection( p.playerID, "Divider", (short)p.level.divider, (short)0, (short)0, (short)(p.level.divider + 1), (short)p.level.depth, (short)p.level.height, Colors.getRGB( "&7" ).r, Colors.getRGB( "&7" ).g, Colors.getRGB( "&7" ).b, (short)127 );
                Thread.Sleep( 3000 );
                p.SendDeleteSelection( p.playerID );
            } ) );
            dividerSelection.Start();
        }

        public override void Help( Player p ) {
            p.SendMessage( "/setdivider - sets the divider between the two teams." );
        }
    }
}
