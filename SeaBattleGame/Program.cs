using SeaBattleGame;
using SeaBattleGame.Mechanics;

Console.OutputEncoding = System.Text.Encoding.Unicode;

/*=====================================================================================================//
//                                          DESCRIPTION                                                //
//=====================================================================================================//

//             Игра.Морской бой (V_1.0)

//В игре участвуют два объекта: корабль и подводная лодка.
//На корабле установлено 16 торпед. Подводная лодка в состоянии выдержать 4 попадания.
//После 4 попадания лодка полностью уничтожается.
//При запуске на экране отображаются корабль (на корабле показано количество торпед)
//и подводная лодка (на подводной лодке показано количество жизней).
//Корабль и подводная лодка постоянно перемещаются из одного края экрана в другой, туда и обратно.
//Скорость перемещения обоих объектов всегда разная.
//Подводная лодка дополнительно меняет глубину тоже каждый раз случайным образом.
//При нажатии на кнопку "вниз на клавишах управления курсором" корабль сбрасывает торпеду.

//Требования к реализации:
// - В программе необходимо использовать 3 дополнительных потока.
// - Первый поток отвечает за движение корабля
// - Второй поток за движение подводной лодки
// - Третий поток за движение торпеды
// (!) Создавать каждый раз поток для запуска торпеды нельзя.
//=====================================================================================================*/

Console.CursorVisible = false;


BattleField.Create();

Thread.Sleep(100);

Thread t1 = new Thread(Ship);
t1.Start();

Thread t2 = new Thread(Submarine);
t2.Start();

Thread t3 = new Thread(Bomb);
t3.Start();

while (true)
{
    ConsoleKeyInfo keyInfo = Console.ReadKey();

    if (keyInfo.Key != ConsoleKey.DownArrow)
        continue;

    GlobalData.BombStart.Set();
}

//====================================================================================================//



static void Ship()
{
    int left = 0;
    int speed = 100;


    int direction = 1;
    while (true)
    {
        lock (SharedResources.Console)
        {
            BattleField.PrintFigure(left, GlobalData.ShipSettings.Top, GlobalData.AIR_COLOR, GlobalData.ShipSettings.Size, " ");

            left += direction;
            GlobalData.ShipSettings.Left = left;

            BattleField.PrintFigure(left, GlobalData.ShipSettings.Top, GlobalData.ShipSettings.Color, GlobalData.ShipSettings.Size, GlobalData.ShipSettings.BombCount.ToString());
        }

        if (left + GlobalData.ShipSettings.Size >= GlobalData.BATTLE_FIELD_WIDTH || left == 0)
        {
            direction *= -1;
            speed = Random.Shared.Next(50, 300);
        }

        Thread.Sleep(speed);
    }
}


static void Submarine()
{
    int left = 0;
    int speed = 100;
    int top = 10;

    int direction = 1;
    int oldTop = top;
    while (true)
    {
        lock (SharedResources.Console)
        {
            BattleField.PrintFigure(left, oldTop, GlobalData.WATER_COLOR, GlobalData.SubmarineSettings.Size, " ");

            left += direction;

            lock (SharedResources.Submarine)
            {
                GlobalData.SubmarineSettings.Left = left;
                GlobalData.SubmarineSettings.Top = top;
            }


            BattleField.PrintFigure(left, top, GlobalData.SubmarineSettings.Color, GlobalData.SubmarineSettings.Size, GlobalData.SubmarineSettings.HP.ToString());
        }

        oldTop = top;
        if (left + GlobalData.SubmarineSettings.Size >= GlobalData.BATTLE_FIELD_WIDTH || left == 0)
        {
            direction *= -1;
            speed = Random.Shared.Next(20, 200);
            top = Random.Shared.Next(9, 14);
        }

        if (GlobalData.SubmarineSettings.HP == 0)
        {
            Console.CursorVisible = true;
            return;
        }

        Thread.Sleep(speed);
    }
}


static void Bomb()
{
    while (true)
    {
        if (GlobalData.ShipSettings.BombCount == 0)
            return;

        GlobalData.BombStart.WaitOne();

        GlobalData.ShipSettings.BombCount--;

        int left = GlobalData.ShipSettings.Left + (GlobalData.ShipSettings.Size / 2);
        int top = GlobalData.ShipSettings.Top + 1;

        int oldTop = top;
        while (true)
        {
            lock (SharedResources.Console)
            {
                BattleField.PrintBomb(left, oldTop, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);

                BattleField.PrintBomb(left, top, GlobalData.WATER_COLOR, ConsoleColor.Yellow);
            }

            Monitor.Enter(SharedResources.Submarine);

            int submarineTop = GlobalData.SubmarineSettings.Top;
            int submarineLeft = GlobalData.SubmarineSettings.Left;

            Monitor.Exit(SharedResources.Submarine);

            if (submarineTop == top && left >= submarineLeft && left <= submarineLeft + GlobalData.SubmarineSettings.Size)
            {
                lock (SharedResources.Console)
                {
                    BattleField.PrintBomb(left, top, GlobalData.WATER_COLOR, GlobalData.WATER_COLOR);
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
                    BattleField.PrintBomb(left, top, GlobalData.SEABED_COLOR, GlobalData.SEABED_COLOR);
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