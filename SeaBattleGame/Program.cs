using SeaBattleGame;
using SeaBattleGame.Mechanics;

Console.OutputEncoding = System.Text.Encoding.Unicode;

/*=====================================================================================================//
//                                          DESCRIPTION                                                //
//=====================================================================================================//

//             Игра.Морской бой (V_2.0)

//В игре участвуют два объекта: корабль и подводная лодка.
//У обоих объектов бесконечное количество снарядов, но только 7 HP
//При запуске на экране отображаются корабль и подводная лодка (уровень здоровья указан на них).
//Корабль и подводная лодка постоянно перемещаются из одного края экрана в другой, туда и обратно.
//Скорость перемещения обоих объектов всегда разная.
//Подводная лодка дополнительно меняет глубину тоже каждый раз случайным образом.
//При нажатии на кнопку "вниз" на клавишах управления курсором, корабль сбрасывает бомбу.
//При нажатии на кнопку "вверх" на клавишах управления курсором, подлобка запускает торпеду.


//Требования к реализации:
// - В программе необходимо использовать 3 дополнительных потока.
// - Первый поток отвечает за движение корабля
// - Второй поток за движение подводной лодки
// - Третий поток за движение торпеды
// (!) Создавать каждый раз поток для запуска торпеды нельзя.
//=====================================================================================================*/

BattleField.PrintIntrodution();

Console.CursorVisible = false;


BattleField.Create();

Thread.Sleep(100);

Thread t1 = new Thread(Ship);
t1.Start();

Thread t2 = new Thread(Submarine);
t2.Start();

Thread t3 = new Thread(Bomb);
t3.Start();

Thread t4 = new Thread(Torpedo);
t4.Start();

while (true)
{
    ConsoleKeyInfo keyInfo = Console.ReadKey();

    if (keyInfo.Key == ConsoleKey.DownArrow)
        GlobalData.BombStart.Set();
    else if(keyInfo.Key == ConsoleKey.UpArrow)
        GlobalData.TorpedoStart.Set();
}

//====================================================================================================//



static void Ship()
{
    int speed = 100;

    int direction = 1;
    do
    {
        lock (SharedResources.Console)
        {
            BattleField.PrintFigure(GlobalData.ShipSettings.Left, GlobalData.ShipSettings.Top, GlobalData.AIR_COLOR, GlobalData.ShipSettings.Size, " ");

            GlobalData.ShipSettings.Left += direction;
            GlobalData.ShipSettings.Left = GlobalData.ShipSettings.Left;

            BattleField.PrintFigure(GlobalData.ShipSettings.Left, GlobalData.ShipSettings.Top, GlobalData.ShipSettings.Color, GlobalData.ShipSettings.Size, GlobalData.ShipSettings.HP.ToString());
        }

        if (GlobalData.ShipSettings.Left + GlobalData.ShipSettings.Size >= GlobalData.BATTLE_FIELD_WIDTH || GlobalData.ShipSettings.Left == 0)
        {
            direction *= -1;
            speed = Random.Shared.Next(50, 300);
        }

        if (GlobalData.ShipSettings.HP == 0)
        {
            return;
        }

        Thread.Sleep(speed);
    } while (true);
}


static void Submarine()
{
    int speed = 100;

    int direction = 1;
    int oldTop = GlobalData.SubmarineSettings.Top;

    while (true)
    {
        lock (SharedResources.Console)
        {
            BattleField.PrintFigure(GlobalData.SubmarineSettings.Left, oldTop, GlobalData.WATER_COLOR, GlobalData.SubmarineSettings.Size, " ");

            GlobalData.SubmarineSettings.Left += direction;

            BattleField.PrintFigure(GlobalData.SubmarineSettings.Left, GlobalData.SubmarineSettings.Top, GlobalData.SubmarineSettings.Color, GlobalData.SubmarineSettings.Size, GlobalData.SubmarineSettings.HP.ToString());
        }

        oldTop = GlobalData.SubmarineSettings.Top;
        if (GlobalData.SubmarineSettings.Left + GlobalData.SubmarineSettings.Size >= GlobalData.BATTLE_FIELD_WIDTH || GlobalData.SubmarineSettings.Left == 0)
        {
            direction *= -1;
            speed = Random.Shared.Next(20, 200);
            GlobalData.SubmarineSettings.Top = Random.Shared.Next(9, 14);
        }

        if (GlobalData.SubmarineSettings.HP == 0)
        {
            return;
        }

        Thread.Sleep(speed);
    }
}


static void Bomb()
{
    Thread.Sleep(10);

    while (true)
    {
        GlobalData.BombStart.WaitOne();

        if (GlobalData.ShipSettings.HP == 0)
            return;

        int left = GlobalData.ShipSettings.Left + (GlobalData.ShipSettings.Size / 2);
        int top = GlobalData.ShipSettings.Top + 1;

        int oldTop = top;
        while (true)
        {
            lock (SharedResources.Console)
            {
                BattleField.PrintMissile(left, oldTop, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);

                BattleField.PrintMissile(left, top, GlobalData.WATER_COLOR, ConsoleColor.Yellow, "⋓");
            }

            Monitor.Enter(SharedResources.Submarine);

            int submarineTop = GlobalData.SubmarineSettings.Top;
            int submarineLeft = GlobalData.SubmarineSettings.Left;

            Monitor.Exit(SharedResources.Submarine);

            if (submarineTop == top && left >= submarineLeft && left <= submarineLeft + GlobalData.SubmarineSettings.Size)
            {
                lock (SharedResources.Console)
                {
                    BattleField.PrintMissile(left, top, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);
                }

                lock (SharedResources.Submarine)
                {
                    GlobalData.SubmarineSettings.HP--;
                }
                break;
            }
            else if (top == GlobalData.AIR_ZONE_HEIGHT + GlobalData.WATER_ZONE_HEIGHT)
            {
                lock (SharedResources.Console)
                {
                    BattleField.PrintMissile(left, top, GlobalData.SEABED_COLOR, GlobalData.SEABED_COLOR);
                }
                break;
            }

            oldTop = top;
            top++;

            Thread.Sleep(300);
        }

        GlobalData.BombStart.Reset();
    }
}

static void Torpedo()
{
    Thread.Sleep(10);

    while (true)
    {
        GlobalData.TorpedoStart.WaitOne();

        if (GlobalData.SubmarineSettings.HP == 0)
            return;

        int left = GlobalData.SubmarineSettings.Left + (GlobalData.SubmarineSettings.Size / 2);
        int top = GlobalData.SubmarineSettings.Top - 1;

        int oldTop = top;
        while (true)
        {
            lock (SharedResources.Console)
            {
                BattleField.PrintMissile(left, oldTop, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);

                BattleField.PrintMissile(left, top, GlobalData.WATER_COLOR, ConsoleColor.Yellow, "⋒");
            }

            Monitor.Enter(SharedResources.Ship);

            int shipTop = GlobalData.ShipSettings.Top;
            int shipLeft = GlobalData.ShipSettings.Left;

            Monitor.Exit(SharedResources.Ship);

            if (shipTop == top && left >= shipLeft && left <= shipLeft + GlobalData.ShipSettings.Size)
            {
                lock (SharedResources.Console)
                {
                    BattleField.PrintMissile(left, top, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);
                }

                lock (SharedResources.Ship)
                {
                    GlobalData.ShipSettings.HP--;
                }
                break;
            }
            else if (top == GlobalData.AIR_ZONE_HEIGHT - 1)
            {
                lock (SharedResources.Console)
                {
                    BattleField.PrintMissile(left, top, GlobalData.AIR_COLOR, GlobalData.AIR_COLOR);
                }
                break;
            }

            oldTop = top;
            top--;

            Thread.Sleep(300);
        }

        GlobalData.TorpedoStart.Reset();
    }
}