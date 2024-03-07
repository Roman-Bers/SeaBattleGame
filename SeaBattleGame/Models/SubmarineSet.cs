using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleGame.Models
{
    internal class SubmarineSet
    {
        public int Size { get; }
        public int Left { get; set; }
        public int Top { get; set; }
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
                        return GlobalData.WATER_COLOR;
                }
            }
        }

        public SubmarineSet()
        {
            Size = 15;

            Left = 0;
            Top = Random.Shared.Next(9, 14);
            HP = 7;
        }
    }
}
