using System;
using NLog;

namespace EFCoreMovies
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            Console.WriteLine("Hello World!");

            logger.Info("Program ended");
        }
    }
}
