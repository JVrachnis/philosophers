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
        public Stopwatch EatingStopWatch { get { return eatingStopWatch; } }
        AverageTimeStopWatch waitStopWatch = new AverageTimeStopWatch();
        public AverageTimeStopWatch WaitStopWatch { get { return waitStopWatch; } }

       
        int timeToEat;
        public delegate void CallAte(Philosopher p);
        CallAte callAte;
        int id;
        public int ID { get { return id; } }
        private bool ate = false;
        public bool Ate { get { return ate; } }
        private Thread philosopherThread;
        public Thread PhilosopherThread { get { return philosopherThread; } }
        public Philosopher(int id, CallAte callAte)
        {
            this.id = id;
            this.callAte = callAte;
            this.timeToEat = 1000 * (id + 1);
            philosopherThread = new Thread(eating);
            philosopherThread.Name = "Philosopher " + id;
            waitStopWatch.Start();
            eatingStopWatch.Stop();
        }
        public bool canEat(bool[] forks)
        {
            if ((forks[id] || forks[(id + 1) % forks.Length]))
            {
                if (forks[id])
                {
                    Console.WriteLine("philosopher: " + id + " FAILED to take fork: " + id + " at " + DateTime.Now.ToString("h:mm:ss tt"));
                }
                if (forks[(id+1) % forks.Length])
                {
                    Console.WriteLine("philosopher: " + id + " FAILED to take fork: " + ((id + 1) % forks.Length) + " at " + DateTime.Now.ToString("h:mm:ss tt"));
                }
                return false;
            }
            return true;
        }
        public void startEating()
        {
            philosopherThread = new Thread(eating);
            philosopherThread.Name = "Philosopher " + id;
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
            callAte(this);
        }
    }
}
