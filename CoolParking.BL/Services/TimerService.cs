using CoolParking.BL.Interfaces;
using System;
using System.Timers;
namespace CoolParking.BL.Services
{
    public class TimerService : ITimerService, IDisposable
    {
        readonly Timer timer;

        public event ElapsedEventHandler Elapsed;

        public double Interval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        public TimerService()
        {
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke(sender, e);
        }
    }

}