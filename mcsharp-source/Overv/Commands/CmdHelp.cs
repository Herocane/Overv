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
	public class CmdHelp : Command {
        public override string name { get { return "help"; } }
        public override string type { get { return "info"; } }
        public override LevelPermission defaultPerm { get { return LevelPermission.Guest; } }
		public CmdHelp() {  }
        public override void Use( Player p, string message ) {
            if ( message != "" ) {
                if ( Command.all.Find( message ) != null ) {
                    Command.all.Find( message ).Help( p );
                    p.SendMessage( "Rank needed: " + GetColor( Command.all.Find( message ) ) + Group.commandPerms.Find( cmda => cmda.cmd == Command.all.Find( message ) ).perm );
                } else if ( Block.Byte( message ) != Block.Zero ) {
                    p.SendMessage( "Block &b" + Block.Name( Block.Byte( message ) ) + "&S appears as &3" + Block.Name( Block.Convert( Block.Byte( message ) ) ) );
                } else {
                    p.SendMessage( "Could not find specified command or block \"" + message + "\"." );
                }

                return;
            }

            retry:
            if ( File.Exists( "text/help.txt" ) ) {
                foreach ( string line in File.ReadAllLines( "text/help.txt" ) ) {
                    p.SendMessage( line );
                }
            } else {
                StreamWriter sw = new StreamWriter( File.Create( "text/help.txt" ) );
                sw.WriteLine( "&d(This message is customisable in help.txt!)\r\n&f- &SRun to the other side and walk through the flag pole to take their flag, then run back to base and do the same to capture their flag.\r\n&f- &SBear in mind that your flag MUST be at your base to capture the other teams flag!\r\n&f- &STo kill players, place tnt and explode by placing a purple block, or place a mine by placing a dark grey block and hope that someone runs into it!" );
                sw.Flush();
                sw.Close();
                sw.Dispose();
                goto retry;
            }
        }

        public string GetColor( Command cmd ) {
            foreach ( Group.CommandAllowance cmda in Group.commandPerms ) {
                if ( cmda.cmd.name == cmd.name ) {
                    if ( Group.Find( cmda.perm ) == null ) {
                        return Group.defaultGroup.color;
                    } else {
                        return Group.Find( cmda.perm ).color;
                    }
                }
            }
            return Group.defaultGroup.color;
        }

        public override void Help(Player p)  
        {
			p.SendMessage("/help [command] - Shows a list of commands or more detail for a specific command.");
		}
	}
}