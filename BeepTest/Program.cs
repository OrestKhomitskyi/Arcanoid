using System;
using System.Threading.Tasks;

namespace BeepTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeABeep();
            Console.ReadLine();
        }

        static async void MakeABeep()
        {
            while (true)
            {
                Console.Beep(90, 10);
                await Task.Delay(10);
            }
        }
    }
}
