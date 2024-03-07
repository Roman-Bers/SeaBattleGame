using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleGame;
using static System.Net.Mime.MediaTypeNames;


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

            _printCentralAlign("М О Р С К О Й   Б О Й", 0);

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

        public static void PrintIntrodution()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int lineCounter = 0;
            _printCentralAlign("И Г Р А  \"М О Р С К О Й   Б О Й\"", ++lineCounter);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            lineCounter += 2;

            _printCentralAlign("В игре участвуют два объекта: корабль и подводная лодка", lineCounter++, 25);
            _printCentralAlign("У обоих объектов бесконечное количество снарядов, но только 7 HP", lineCounter++, 25);
            lineCounter++;

            _printCentralAlign("При запуске на экране отображаются корабль и подводная лодка (уровень здоровья указан на них)", lineCounter++, 25);
            _printCentralAlign("Корабль и подводная лодка постоянно перемещаются из одного края экрана в другой, туда и обратно.", lineCounter++, 25);
            _printCentralAlign("Скорость перемещения обоих объектов всегда разная.", lineCounter++, 25);
            _printCentralAlign("Подводная лодка дополнительно меняет глубину тоже каждый раз случайным образом.", lineCounter++, 25);
            lineCounter++;

            _printCentralAlign("При нажатии на кнопку \"ВНИЗ\" на клавишах управления курсором, корабль сбрасывает бомбу.", lineCounter++, 25);
            _printCentralAlign("При нажатии на кнопку \"ВВЕРХ\" на клавишах управления курсором, подлобка запускает торпеду.", lineCounter++, 25);
            lineCounter++;

            Thread.Sleep(30);
            _printCentralAlign("Для продолжения нажмите любую кравишу", lineCounter);
            Console.ReadKey();

            Console.ResetColor();
            Console.Clear();
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


        public static void PrintMissile(int left, int top, ConsoleColor bgColor, ConsoleColor fgColor, string text = "⊛")
        {
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

        private static void _printCentralAlign(string text, int top, int drawSpeed = 0)
        {
            Console.SetCursorPosition((GlobalData.BATTLE_FIELD_WIDTH - text.Length) / 2, top);
            foreach (char c in text) 
            { 
                Console.Write(c);
                Thread.Sleep(drawSpeed);
            }
        }
    }
}
