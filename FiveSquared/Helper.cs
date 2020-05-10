using System;
using System.Threading; 

namespace FiveSquared
{
    public class Helper
    {
        // Default constructor
        public Helper()
        {
        
        }

        public static void Pause(int numSecs)
        {
            Console.WriteLine("Pausing {0} seconds...", numSecs);
            Thread.Sleep(numSecs * 100);
        }

        public static void PausePressAnyKey()
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}