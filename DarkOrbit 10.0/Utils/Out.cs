using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Utils
{
    class Out
    {
        public static void WriteLine(string message, string header = "", ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[" + DateTime.Now + "]");

            if (header != "")
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("]");
            }

            Console.Write(" - ");
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
