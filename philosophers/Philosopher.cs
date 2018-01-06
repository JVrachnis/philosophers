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
    class Philosopher
    {
        const long maxEatTime = 20000;

        Stopwatch eatingStopWatch = new Stopwatch();
        AverageTimeStopWatch waitStopWatch = new AverageTimeStopWatch();

        public AverageTimeStopWatch StopWatchTime { get { return waitStopWatch; } }
        public Stopwatch EatingStopWatch { get { return eatingStopWatch; } }
        int timeToEat;
        public delegate void CallEat(Philosopher p);
        CallEat callEat;
        int id;
        public int ID { get { return id; } }
        string name;
        public string Name { get { return name; } }
        private Thread philosopherThread;
        private bool ate = false;
        public bool Ate { get { return ate; } }
        public Thread PhilosopherThread { get { return philosopherThread; } }
        public Philosopher(int id, string name, CallEat callEat)
        {
            this.callEat = callEat;
            this.id = id;
            this.name = name;
            this.timeToEat = 1000 * (id + 1);
            philosopherThread = new Thread(eating);
            philosopherThread.Name = name;
            waitStopWatch.Start();
            eatingStopWatch.Stop();
        }
        public bool canEat(bool[] forks)
        {
            if ((forks[id] || forks[id % forks.Length]))
            {
                if (forks[id])
                {
                    Console.WriteLine("philosopher: " + id + " FAILED to take fork: " + id + " at " + DateTime.Now.ToString("h:mm:ss tt"));
                }
                if (forks[id % forks.Length])
                {
                    Console.WriteLine("philosopher: " + id + " FAILED to take fork: " + (id % forks.Length) + " at " + DateTime.Now.ToString("h:mm:ss tt"));
                }
                return false;
            }
            return true;
        }
        public void startEating()
        {
            philosopherThread = new Thread(eating);
            philosopherThread.Name = name;
            philosopherThread.Start();
        }
        public void eating()
        {
            Stopwatch stopWatch = new Stopwatch();
            waitStopWatch.StopA();
            eatingStopWatch.Start();
            stopWatch.Start();
            double timeStartedEating = stopWatch.ElapsedMilliseconds;

            Console.WriteLine("philosopher: " + id + " started EATING at " + DateTime.Now.ToString("h:mm:ss tt"));
            while (stopWatch.ElapsedMilliseconds - timeStartedEating < timeToEat)
            {
                if (eatingStopWatch.ElapsedMilliseconds >= maxEatTime)
                {
                    break;
                }
                Thread.Sleep(0);
            }
            eatingStopWatch.Stop();
            if (eatingStopWatch.ElapsedMilliseconds < maxEatTime)
            {
                waitStopWatch.Start();
                Console.WriteLine("philosopher: " + id + " EATING for " + eatingStopWatch.Elapsed);
                Console.WriteLine("philosopher: " + id + " started THINKING at " + DateTime.Now.ToString("h:mm:ss tt"));
            }
            else
            {
                Console.WriteLine("philosopher: " + id + " ATE at " + DateTime.Now.ToString("h:mm:ss tt"));
                ate = true;
            }
            callEat(this);
        }
    }
}
