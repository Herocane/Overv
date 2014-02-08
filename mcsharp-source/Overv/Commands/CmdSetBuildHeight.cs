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
    class CmdSetBuildHeight : Command {
        public override string name { get { return "setbuildheight"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
        public CmdSetBuildHeight() { }

        public override void Use( Player p, string message ) {
            p.SendMessage( "Place a block to determine the max build height." );
            p.Blockchange += Blockchange;
        }

        public void Blockchange( Player p, ushort x, ushort y, ushort z, byte type ) {
            byte b = p.level.GetBlock( x, y, z );
            p.SendBlockchange( x, y, z, b );
            p.ClearBlockchange();

            p.level.maxBuildHeight = y;

            p.SendMessage( "Max build height set!" );
        }

        public override void Help( Player p ) {
            p.SendMessage( "/setbuildheight - sets the maximum build height for this level." );
        }
    }
}
