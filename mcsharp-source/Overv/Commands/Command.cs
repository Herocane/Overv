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
    public abstract class Command {
        public abstract string name { get; }
        public abstract string type { get; }
        public abstract LevelPermission defaultPerm { get; }
        public abstract void Use( Player p, string message );
        public abstract void Help( Player p );

        public static CommandList all = new CommandList();
        public static void Load() {
            all.Add( new CmdBan() );
            all.Add( new CmdBanip() );
            all.Add( new CmdGag() );
            all.Add( new CmdGoto() );
            all.Add( new CmdHelp() );
            all.Add( new CmdInfo() );
            all.Add( new CmdKick() );
            all.Add( new CmdLevels() );
            all.Add( new CmdLoad() );
            all.Add( new CmdPlayers() );
            all.Add( new CmdSave() );
            all.Add( new CmdSetSpawn() );
            all.Add( new CmdSummon() );
            all.Add( new CmdTp() );
            all.Add( new CmdUnban() );
            all.Add( new CmdUnbanip() );
            all.Add( new CmdUnload() );
            all.Add( new CmdAfk() );
            all.Add( new CmdRules() );
            all.Add( new CmdRestore() );
            all.Add( new CmdPermissionBuild() );
            all.Add( new CmdPermissionVisit() );
            all.Add( new CmdMove() );
            all.Add( new CmdDrop() );
            all.Add( new CmdBlueTeam() );
            all.Add( new CmdRedTeam() );
            all.Add( new CmdSetRank() );
            all.Add( new CmdDefuse() );
            all.Add( new CmdLike() );
            all.Add( new CmdDislike() );
            all.Add( new CmdSetBuildHeight() );
        }
    }
}