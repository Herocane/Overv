using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Overv {
    public class PlayerDB {
        public static void Load( Player p ) {
            string file = p.name + ".database";

            if ( File.Exists( file ) ) {
                foreach ( string line in File.ReadAllLines( file ) ) {
                    if ( !string.IsNullOrEmpty( line ) && !line.StartsWith( "#" ) ) {
                        string property = line.Split( '=' )[0].Trim();
                        string value = line.Split( '=' )[1].Trim();

                        switch ( property.ToLower() ) {
                            case "points":
                                p.points = int.Parse( value );
                                break;
                        }
                    }
                }
            } else {
                p.points = 0;
                Save( p );
            }
        }

        public static void Save( Player p ) {
            StreamWriter sw = new StreamWriter( File.Create( p.name + ".database" ) );
            sw.WriteLine( "Points = " + p.points );
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
