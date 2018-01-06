using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections;
namespace philosophers
{
    class Program
    {
        static int PhilosophersCount=0;
        static DinnerTable PhilosophersDinner;
        static void Main(string[] args)
        {
            parseArgs(args);
            PhilosophersDinner = new DinnerTable(PhilosophersCount);
            while (PhilosophersDinner.PhilosophersHungry())
            {
                Thread.Sleep(10);
            }
            PhilosophersDinner.dinnerEnded();
            PhilosophersDinner.printStatistics();
            while (true)
            {
                Thread.Sleep(10);
            }
        }
        static private void parseArgs(string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out PhilosophersCount))
            {
                if (PhilosophersCount < 3 && PhilosophersCount > 10)
                {
                    PhilosophersCount = getPhilosophersCount();
                }
            }
            else
            {
                PhilosophersCount = getPhilosophersCount();
            }
        }
        static private int getPhilosophersCount()
        {
            int PhilosophersCount =0;
            do
            {
                Console.WriteLine("couldnt use that try a number between 3 and 10");
            } while (!(int.TryParse(Console.ReadLine(), out PhilosophersCount) && PhilosophersCount >= 3 && PhilosophersCount <= 10));
            return PhilosophersCount;
        }
        
    }

}