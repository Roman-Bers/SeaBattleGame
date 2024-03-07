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
        public int Left { get; set; }
        public int HP { get; set; }

        public ConsoleColor Color
        {
            get
            {
                switch (HP)
                {
                    case 7:
                        return ConsoleColor.DarkBlue;
                    case 6:
                        return ConsoleColor.Cyan;
                    case 5:
                        return ConsoleColor.DarkGreen;
                    case 4:
                        return ConsoleColor.Green;
                    case 3:
                        return ConsoleColor.DarkYellow;
                    case 2:
                        return ConsoleColor.DarkMagenta;
                    case 1:
                        return ConsoleColor.Magenta;
                    case 0:
                        return ConsoleColor.Red;
                    default:
                        return GlobalData.AIR_COLOR;
                }
            }
        }



        public ShipSet()
        {
            Size = 10;
            Top = GlobalData.AIR_ZONE_HEIGHT - 1;

            Left = Random.Shared.Next(0, GlobalData.BATTLE_FIELD_WIDTH - Size - 2);
            HP = 7;
        }
    }
}
