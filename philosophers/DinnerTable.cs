using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections;
namespace philosophers
{
    class DinnerTable
    {
        static Stopwatch stopWatch = new Stopwatch();
        static private bool[] forks;
        static private ArrayList philosophersHungry;
        static private List<Philosopher> philosophersHungryRemove = new List<Philosopher>();
        static private List<Philosopher> philosophers;
        public DinnerTable(int PhilosophersCount)
        {
            philosophersHungry = new ArrayList();
            philosophers = new List<Philosopher>();
            forks = new bool[PhilosophersCount];
            for (int i = 0; i < PhilosophersCount; i++)
            {
                philosophers.Add(new Philosopher(i, "philo" + (i), philosopherStopedEating));
                philosophersHungry.Add(philosophers.ElementAt(i));
            }
            FindNext();
            stopWatch.Start();
        }
        static private void FindNext()
        {
            foreach (Philosopher p in philosophersHungry)
            {
                StartIfCanEat(p);
            }
            foreach (Philosopher p in philosophersHungryRemove)
            {
                philosophersHungry.Remove(p);
            }
            philosophersHungryRemove.Clear();
        }
        static private void StartIfCanEat(Philosopher p)
        {
            if (p.canEat(forks) && !p.Ate)
            {
                p.startEating();
                setUseForks(p, true);
                philosophersHungryRemove.Add(p);

            }
        }
        static private void setUseForks(Philosopher p, bool used)
        {
            forks[p.ID] = used;
            forks[p.ID % forks.Length] = used;
        }
        static private void philosopherStopedEating(Philosopher p)
        {
            lock (philosophersHungry)
            {
                setUseForks(p, false);
                if (!p.Ate)
                {
                    philosophersHungry.Add(p);
                }
                FindNext();
            }
        }
        public bool PhilosophersHungry()
        {
            return philosophers.Exists(p => !p.Ate);
        }
        public void dinnerEnded()
        {
            
            stopWatch.Stop();
        }
        public void printStatistics()
        {
            Console.WriteLine("all philosophers Ate .Diner lasted for " + stopWatch.Elapsed);
            Console.WriteLine("average Thinking times: ");
            foreach (Philosopher p in philosophers)
            {
                Console.WriteLine(p.Name + ": " + p.StopWatchTime.AverageElapsed);
            }
            Console.WriteLine("Thinking times: ");
            foreach (Philosopher p in philosophers)
            {
                Console.WriteLine(p.Name + ": " + p.StopWatchTime.Elapsed);
            }
            Console.WriteLine("Eating times: ");
            foreach (Philosopher p in philosophers)
            {
                Console.WriteLine(p.Name + ": " + p.EatingStopWatch.Elapsed);
            }
        }
    }
}
