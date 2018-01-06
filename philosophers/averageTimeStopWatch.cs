using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace philosophers
{
    class AverageTimeStopWatch:Stopwatch
    {
        int timesStopped=0;
        public double AverageElapsedMilliseconds {
            get { return timesStopped == 0? (ElapsedMilliseconds):(ElapsedMilliseconds / timesStopped);}
        }
        public TimeSpan AverageElapsed
        {
            get { return TimeSpan.FromMilliseconds(timesStopped == 0 ? (ElapsedMilliseconds) : (ElapsedMilliseconds / timesStopped)); }
        }
        public double AverageElapsedTicks
        {
            get { return timesStopped == 0 ? (ElapsedTicks) : (ElapsedTicks / timesStopped); }
        }
        public AverageTimeStopWatch(): base()
        {
            timesStopped = 0;
        }
        public void StopA()
        {
            this.Stop();
            timesStopped++;
        }
    }
}