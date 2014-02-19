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
	public static class Colors {
		public const string black = "&0";
		public const string navy = "&1";
		public const string green = "&2";
		public const string teal = "&3";
		public const string maroon = "&4";
		public const string purple = "&5";
		public const string gold = "&6";
		public const string silver = "&7";
		public const string gray = "&8";
		public const string blue = "&9";
		public const string lime = "&a";
		public const string aqua = "&b";
		public const string red = "&c";
		public const string pink = "&d";
		public const string yellow = "&S";
		public const string white = "&f";

		public static string Parse(string str) {
			switch (str.ToLower()) {
					case "black": return black;
					case "navy": return navy;
					case "green": return green;
					case "teal": return teal;
					case "maroon": return maroon;
					case "purple": return purple;
					case "gold": return gold;
					case "silver": return silver;
					case "gray": return gray;
					case "blue": return blue;
					case "lime": return lime;
					case "aqua": return aqua;
					case "red": return red;
					case "pink": return pink;
					case "yellow": return yellow;
					case "white": return white;
					default: return "";
			}
		} 
        
        public static string Name(string str) {
			switch (str) {
					case black: return "black";
					case navy: return "navy";
					case green: return "green";
					case teal: return "teal";
					case maroon: return "maroon";
					case purple: return "purple";
					case gold: return "gold";
					case silver: return "silver";
					case gray: return "gray";
					case blue: return "blue";
					case lime: return "lime";
					case aqua: return "aqua";
					case red: return "red";
					case pink: return "pink";
					case yellow: return "yellow";
					case white: return "white";
					default: return "";
			}
		} 
        
        public static RGB getRGB( string str ) {
            switch ( str ) {
                case black: return new RGB( 0, 0, 0 );
                case navy: return new RGB( 0, 0, 170 );
                case green: return new RGB( 0, 170, 0 );
                case teal: return new RGB( 0, 170, 170 );
                case maroon: return new RGB( 170, 0, 0 );
                case purple: return new RGB( 170, 0, 170 );
                case gold: return new RGB( 170, 170, 0 );
                case silver: return new RGB( 170, 170, 170 );
                case gray: return new RGB( 85, 85, 85 );
                case blue: return new RGB( 85, 85, 255 );
                case lime: return new RGB( 85, 255, 85 );
                case aqua: return new RGB( 85, 255, 255 );
                case red: return new RGB( 255, 85, 85 );
                case pink: return new RGB( 255, 85, 255 );
                case yellow: return new RGB( 255, 255, 85 );
                case white: return new RGB( 255, 255, 255 );
                default: return new RGB(255, 255, 255);
            }
        }
	}

    public class RGB {
        public short r;
        public short g;
        public short b;

        public RGB( short red, short green, short blue ) {
            r = red;
            g = green;
            b = blue;
        }
    }
}