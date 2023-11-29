using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Мотосалон
{
    internal class Arrow
    {

        public static int Show(int min, int max, int f_key = -1)
        {
            int pos = min;
            ConsoleKeyInfo key;
            do
            {
                if (min <= max)
                {
                    Console.SetCursorPosition(0, pos);
                    Console.WriteLine("->");
                }
                key = Console.ReadKey(true);
                if (min <= max)
                {
                    Console.SetCursorPosition(0, pos);
                    Console.WriteLine("  ");
                    if (key.Key == ConsoleKey.UpArrow && pos != min)
                    {
                        pos--;
                    }
                    else if (key.Key == ConsoleKey.DownArrow && pos != max)
                    {
                        pos++;
                    }
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    return (int)Keys.Escape;
                }
                else if (f_key >= (int)Keys.F1)
                {
                    if (f_key == (int)Keys.Subtract)
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.F2:
                                return (int)Keys.F2 + max;
                            case ConsoleKey.Add:
                                return (int)Keys.Add + max;
                            case ConsoleKey.Subtract:
                                return (int)Keys.Subtract + max;
                        }
                    }
                    else
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.F1:
                                return (int)Keys.F1 + max;
                            case ConsoleKey.F2:
                                return (int)Keys.F2 + max;
                            case ConsoleKey.F3:
                                return (int)Keys.F3 + max;
                        }
                    }
                }
            } while (key.Key != ConsoleKey.Enter || min > max);
            return pos;
        }
    }
}