using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleGame.Models
{
    internal class ShipSet
    {
        public int Size { get; }
        public int Top { get; }
        public ConsoleColor Color { get; }

        public int Left { get; set; }

        public int BombCount { get; set; }

        public ShipSet()
        {
            Size = 15;
            Top = GlobalData.AIR_ZONE_HEIGHT - 1;

            Color = ConsoleColor.DarkGreen;
            Left = 0;
            BombCount = 16;
        }
    }
}
