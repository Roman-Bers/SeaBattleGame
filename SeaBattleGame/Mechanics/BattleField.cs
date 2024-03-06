using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleGame;


namespace SeaBattleGame.Mechanics
{
    internal static class BattleField
    {
        static BattleField()
        { }

        public static void Create()
        {
            int gameZoneHeight = GlobalData.AIR_ZONE_HEIGHT + GlobalData.WATER_ZONE_HEIGHT + GlobalData.SEABED_ZONE_HEIGHT;

            Console.SetWindowSize(GlobalData.BATTLE_FIELD_WIDTH, gameZoneHeight);
            Console.SetBufferSize(GlobalData.BATTLE_FIELD_WIDTH, gameZoneHeight);

            Console.BackgroundColor = GlobalData.AIR_COLOR;
            Console.ForegroundColor = ConsoleColor.Black;

            _printArea(GlobalData.AIR_ZONE_HEIGHT);

            string str = "М О Р С К О Й   Б О Й";
            Console.SetCursorPosition((GlobalData.BATTLE_FIELD_WIDTH - str.Length) / 2, 0);
            Console.WriteLine(str);

            Console.ResetColor();

            Console.SetCursorPosition(0, GlobalData.AIR_ZONE_HEIGHT);
            Console.BackgroundColor = GlobalData.WATER_COLOR;

            _printArea(GlobalData.WATER_ZONE_HEIGHT);

            Console.ResetColor();

            Console.SetCursorPosition(0, GlobalData.AIR_ZONE_HEIGHT + GlobalData.WATER_ZONE_HEIGHT);
            Console.BackgroundColor = GlobalData.SEABED_COLOR;

            _printArea(GlobalData.SEABED_ZONE_HEIGHT);

            Console.ResetColor();
            //Console.SetCursorPosition(0,0);
        }

        public static void PrintFigure(int left, int top, ConsoleColor color, int size, string centerText)
        {
            int padding = (size / 2 - 1);
            string figureText = $"{new string(' ', padding)}{centerText, 2}{new string(' ', padding)}";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = color;
            Console.SetCursorPosition(left, top);
            Console.Write(figureText);
        }


        public static void PrintBomb(int left, int top, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            string text = "⊛";
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.SetCursorPosition(left, top);
            Console.Write(text);
        }

        private static void _printArea(int height)
        {
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine(new string(' ', GlobalData.BATTLE_FIELD_WIDTH));
            }
        }
    }
}
