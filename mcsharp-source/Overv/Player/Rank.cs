using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Overv {
    public class Rank {
        public static List<Rank> ranks = new List<Rank>();
        static string ranksFile = "ranks.properties";

        public int id;
        public int requiredxp;

        public Rank( int nid, int nrequiredxp ) {
            id = nid;
            requiredxp = nrequiredxp;
        }

        public static void Load() {
            if ( File.Exists( ranksFile ) ) {
                foreach ( string line in File.ReadAllLines( ranksFile ) ) {
                    if ( !string.IsNullOrEmpty( line ) && !line.StartsWith( "#" ) ) {
                        int fid = int.Parse( line.Split( ':' )[0].Trim() );
                        int frequiredxp = int.Parse( line.Split( ':' )[1].Trim() );
                        ranks.Add( new Rank( fid, frequiredxp ) );
                    }
                }
            } else {

            }

            for ( int i = 0; i <= 20; i++ ) {
                if ( ranks.Find( rnk => rnk.id == i ) == null ) { ranks.Add( new Rank( i, i*i ) ); }
            }
        }

        public static void Save() {
            StreamWriter sw = new StreamWriter( File.Create( ranksFile ) );
            foreach ( Rank rnk in ranks ) {
                sw.WriteLine( rnk.id + " : " + rnk.requiredxp );
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static Rank FindRank( int points ) {
            Rank foundRank = null;
            foreach ( Rank rnk in ranks ) {
                if ( foundRank != null ) {
                    if ( rnk.requiredxp <= points && rnk.requiredxp < foundRank.requiredxp ) {
                        foundRank = rnk;
                    }
                } else {
                    foundRank = rnk;
                }
            }
            return foundRank;
        }
    }
}
