using SeaBattleGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleGame
{
    internal static class GlobalData
    {
        public const int BATTLE_FIELD_WIDTH = 100;

        public const int AIR_ZONE_HEIGHT = 7;
        public const int WATER_ZONE_HEIGHT = 15;
        public const int SEABED_ZONE_HEIGHT = 2;

        public const ConsoleColor AIR_COLOR = ConsoleColor.White;
        public const ConsoleColor WATER_COLOR = ConsoleColor.Blue;
        public const ConsoleColor SEABED_COLOR = ConsoleColor.DarkYellow;

        static public SubmarineSet SubmarineSettings { get; set; }
        static public ShipSet ShipSettings { get; set; }

        static public ManualResetEvent BombStart { get; set; }

        static GlobalData() 
        {
            SubmarineSettings = new SubmarineSet();
            ShipSettings = new ShipSet();
            BombStart = new ManualResetEvent(false);
        }
    }
}
