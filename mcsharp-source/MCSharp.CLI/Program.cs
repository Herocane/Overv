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
using System.IO;
using System.Text;

using Overv;

namespace Overv.CLI {
    class Program {
        static Server server;
        static void Main( string[] args ) {
            server = new Server();
            server.OnLog += Log;
            server.OnSettingsUpdate += SettingsUpdate;
            server.Start();
            Server.ParseInput();
        }

        static void SettingsUpdate() {
            Console.Title = Server.name;
        }

        static void Log( string message ) {
            Console.WriteLine( message );
        }
    }
}
