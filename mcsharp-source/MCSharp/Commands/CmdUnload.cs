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
using System.Threading;

namespace Overv {
	public class CmdUnload : Command {
        public override string name { get { return "unload"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.AdvBuilder; } }
		public CmdUnload() {  }
		public override void Use(Player p,string message)  {
            Server.levels.ForEach( delegate( Level level ) {
                if ( level.name.ToLower() == message.ToLower() ) {
                    if ( level == Server.mainLevel ) { p.SendMessage( "You can't unload the main level." ); return; }
                    Player.players.ForEach( delegate( Player pl ) { if ( pl.level == level ) { Player.GlobalDie( pl, true ); } } );
                    PlayerBot.playerbots.ForEach( delegate( PlayerBot b ) { if ( b.level == level ) { b.GlobalDie(); } } );       //destroy any bots on the level
                    Player.players.ForEach( delegate( Player pl ) { if ( pl.level == level ) { pl.SendMotd(); } } );

                    ushort x = (ushort)( ( 0.5 + Server.mainLevel.spawnx ) * 32 );
                    ushort y = (ushort)( ( 1 + Server.mainLevel.spawny ) * 32 );
                    ushort z = (ushort)( ( 0.5 + Server.mainLevel.spawnz ) * 32 );

                    Player.players.ForEach( delegate( Player pl ) {
                        if ( pl.level == level ) {
                            Command.all.Find( "goto" ).Use( pl, Server.mainLevel.name );
                            Thread.Sleep( 500 );
                        }
                    } );
                    Server.levels.Remove( level );

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Player.GlobalMessage( "&3" + level.name + "&S was unloaded." );
                }
            } );
            p.SendMessage("There is no level \""+message+"\" loaded.");
		} 
        
        public override void Help(Player p)  {
			p.SendMessage("/unload [level] - Unloads a level.");
		}
	}
}