using System;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using OpenNETCF.Windows.Forms;

namespace Application2Console
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            ThreadPool.QueueUserWorkItem(Exit);

            Application2.Run();
        }

        private static void Exit(object o)
        {
            int start = Environment.TickCount;
            bool consoleRead = true;

            if (consoleRead)
            {
                Thread.Sleep(1000);
                Console.Out.WriteLine("Press ENTER to exit the Service Layer.");
                Console.In.ReadLine();
            }
            Application2.Exit();

        }
    }
}