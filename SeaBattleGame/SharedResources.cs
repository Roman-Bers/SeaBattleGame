using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleGame
{
    internal static class SharedResources
    {
        static public object Console { get; set; }
        static public object Submarine { get; set; }

        static SharedResources()
        {
            Console = new object();
            Submarine = new object();
        }
    }
}
