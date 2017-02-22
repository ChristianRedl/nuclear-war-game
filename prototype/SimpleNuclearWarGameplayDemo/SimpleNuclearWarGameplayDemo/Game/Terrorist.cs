using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNuclearWarGameplayDemo.Entities;
using System.Threading;

namespace SimpleNuclearWarGameplayDemo.Game
{
    class Terrorist
    {
        private Object multiThreadLock = new Object();
        private Thread monitorThread = null;
        private volatile bool Halted = false;
        private List<Entities.Random> randomEvents;
        private EventHandler targetHit;
        private World world;

        public Terrorist(List<Entities.Random> randomEvents, EventHandler targetHit, World world)
        {
            this.randomEvents = randomEvents;
            this.targetHit = targetHit;
            this.world = world;

            lock (multiThreadLock)
            {
                if (monitorThread != null) throw new Exception("Previous monitoring has not yet been stopped!");
                monitorThread = new Thread(new ThreadStart(PollRandomEvents));
            }
            Halted = false;
            monitorThread.Start();
        }

        private void PollRandomEvents()
        {
            try { 
                while (!Halted)
                {
                    int timeUntilNextRandomEventMillis = 10000 + GameControl.GetRandom().Next(50000);
                    Thread.Sleep(timeUntilNextRandomEventMillis);
                    Entities.Random action = GameControl.GetRandom(randomEvents);
                    lock (action) {                     
                        action.TargetReached -= targetHit;
                        action.TargetReached += targetHit;
                        action.OnTargetReached(new TargetReachedEventArgs
                        {
                                Region = GameControl.GetRandom(world.Regions)
                        });
                    }
               
                }
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            monitorThread = null;
        }

        internal void Stop()
        {
            Halted = true;
            monitorThread?.Abort();
        }
    }
}
