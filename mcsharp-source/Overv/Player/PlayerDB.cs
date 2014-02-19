using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Overv {
    public class PlayerDB {
        public static void Load( Player p ) {
            string file = "players/" + p.name + ".database";

            if ( File.Exists( file ) ) {
                foreach ( string line in File.ReadAllLines( file ) ) {
                    if ( !string.IsNullOrEmpty( line ) && !line.StartsWith( "#" ) ) {
                        string property = line.Split( '=' )[0].Trim();
                        string value = line.Split( '=' )[1].Trim();

                        switch ( property.ToLower() ) {
                            case "points":
                                p.points = int.Parse( value );
                                break;
                            case "captures":
                                p.totalCaptures = int.Parse( value );
                                break;
                            case "returns":
                                p.totalReturns = int.Parse( value );
                                break;
                            case "kills":
                                p.totalKills = int.Parse( value );
                                break;
                            case "deaths":
                                p.totalDeaths = int.Parse( value );
                                break;
                            case "logins":
                                p.totalLogins = int.Parse( value );
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
            StreamWriter sw = new StreamWriter( File.Create( "players/" + p.name + ".database" ) );
            sw.WriteLine( "Points = " + p.points );
            sw.WriteLine( "Captures = " + p.totalCaptures );
            sw.WriteLine( "Returns = " + p.totalReturns );
            sw.WriteLine( "Kills = " + p.totalKills );
            sw.WriteLine( "Deaths = " + p.totalDeaths );
            sw.WriteLine( "Logins = " + p.totalLogins );
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
