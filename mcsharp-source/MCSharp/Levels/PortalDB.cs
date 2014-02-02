using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Overv {
    public class PortalDB {
        public static List<Portal> portals = new List<Portal>();
        static string portalFile = "portals.properties";

        public static void Load() {
            if ( File.Exists( portalFile ) ) {
                foreach ( string line in File.ReadAllLines( portalFile ) ) {
                    string entrylevel, exitlevel;
                    ushort entx, enty, entz, extx, exty, extz;

                    string[] values = line.Split( ':' );
                    if ( values.Length == 8 ) {
                        entrylevel = values[0];
                        exitlevel = values[1];
                        entx = ushort.Parse(values[2]);
                        enty = ushort.Parse(values[3]);
                        entz = ushort.Parse(values[4]);
                        extx = ushort.Parse(values[5]);
                        exty = ushort.Parse(values[6]);
                        extz = ushort.Parse(values[7]);

                        Portal newPortal = new Portal( entrylevel, exitlevel, entx, enty, entz, extx, exty, extz );
                        portals.Add( newPortal );
                    }
                }
            }

            Server.s.Log( "LOADED: portals.properties..." );
        }

        public static void Save() {
            StreamWriter sw = new StreamWriter( File.Create( portalFile ) );
            portals.ForEach( delegate( Portal p ) {
                sw.WriteLine( p.entryLevel + ":" + p.exitLevel + ":" + p.entry[0] + ":" + p.entry[1] + ":" + p.entry[2] + ":" + p.exit[0] + ":" + p.exit[1] + ":" + p.exit[2] );
            } );
            sw.Flush();
            sw.Close();
            sw.Dispose();

            Server.s.Log( "SAVED: portals.properties..." );
        }
    }

    public class Portal {
        public string entryLevel;
        public string exitLevel;
        public ushort[] entry;
        public ushort[] exit;

        public Portal( string entlvl, string extlvl, ushort entx, ushort enty, ushort entz, ushort extx, ushort exty, ushort extz ) {
            entryLevel = entlvl;
            exitLevel = extlvl;
            entry = new ushort[3] { entx, enty, entz };
            exit = new ushort[3] { extx, exty, extz };
        }
    }
}
