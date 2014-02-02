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

namespace Overv
{
    public class CmdReplaceAll : Command {
        public override string name { get { return "replaceall"; } }
        public override string type { get { return "build"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Operator; } }
        public CmdReplaceAll() { }
        public override void Use( Player p, string message ) {
            int number = message.Split( ' ' ).Length;
            if ( number != 2 ) { Help( p ); return; }

            int pos = message.IndexOf( ' ' );
            string t = message.Substring( 0, pos ).ToLower();
            string t2 = message.Substring( pos + 1 ).ToLower();
            byte type = Block.Byte( t );
            if ( type == 255 ) { p.SendMessage( "There is no block \"" + t + "\"." ); return; }
            byte type2 = Block.Byte( t2 );
            if ( type2 == 255 ) { p.SendMessage( "There is no block \"" + t2 + "\"." ); return; }

            List<Pos> buffer = new List<Pos>();

            for ( ushort xx = 0; xx < p.level.width; xx++ ) {
                for ( ushort yy = 0; yy < p.level.width; yy++ ) {
                    for ( ushort zz = 0; zz < p.level.width; zz++ ) {
                        byte b = p.level.GetTile( xx, yy, zz );
                        if ( b == type ) {
                            BufferAdd( buffer, xx, yy, zz );
                        }
                    }
                }
            }

            if ( p.group.drawLimit < buffer.Count ) {
                p.SendMessage( "Your group's drawlimit is " + p.group.drawLimit + ", you tried to draw " + buffer.Count + "!" );
                return;
            }

            p.SendMessage( buffer.Count.ToString() + " blocks." );
            buffer.ForEach( delegate( Pos po ) {
                p.level.Blockchange( p, po.x, po.y, po.z, type2 );
            } );
        }
        public override void Help( Player p ) {
            p.SendMessage( "/replace [type] [type2] - replace all of type with type2 on current map." );
        }

        void BufferAdd( List<Pos> list, ushort x, ushort y, ushort z ) {
            Pos pos; pos.x = x; pos.y = y; pos.z = z; list.Add( pos );
        }

        struct Pos {
            public ushort x, y, z;
        }
    }
}
