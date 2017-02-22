using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNuclearWarGameplayDemo.Entities;
using System.Threading;

namespace SimpleNuclearWarGameplayDemo.Game
{
    public class AIPlayer : Player
    {
        private Nation nation;
        public int PollInterval_Milliseconds { get; private set; }

        private Object multiThreadLock = new Object();
        private Thread monitorThread = null;
        private volatile bool Halted = false;
        private World world;
        private NewsFeed news;
        private EventHandler targetHit;

        public AIPlayer(Nation nation, World world, EventHandler targetHit, NewsFeed news) : base(nation)
        {
            this.nation = nation;
            this.world = world;
            this.targetHit = targetHit;
            this.news = news;

            PollInterval_Milliseconds = Convert.ToInt32(TimeSpan.FromSeconds(30).TotalMilliseconds);

            lock (multiThreadLock)
            {
                if (monitorThread != null) throw new Exception("Previous monitoring has not yet been stopped!");
                monitorThread = new Thread(new ThreadStart(PollState));
            }
            Halted = false;
            monitorThread.Start();
        }

        public void Stop()
        {
            Halted = true;
            monitorThread?.Abort();
        }

        private void PollState()
        {
            while (!Halted)
            {
                Thread.Sleep(PollInterval_Milliseconds);
                List<Entities.Action> actions = GetAvailableActions();
                List<Entities.Action> active = GetActiveActions();
                Entities.Action action = GameControl.GetRandom(actions);  
                if (action==null)
                {
                    // this ia has no actions left
                    Halted = true;
                    break;
                }              
                action.TargetReached -= targetHit;
                action.TargetReached += targetHit;                
                bool success = Trigger(action, GetTarget(action, world), news);                
            }
            monitorThread = null;
        }

        private Region GetTarget(Entities.Action action, World world)
        {
            if (action is Attack)
            {
                Attack attack = (Attack)action;
                // some attacks have a fixed region
                if (String.IsNullOrEmpty(attack.TargetRegion))
                {
                    Region region;
                    int retry = 5;
                    do
                    {
                        region = GameControl.GetRandom(world.Regions);
                    } while (region == null || (retry-- > 0 && (region.Population < 1 || nation.Regions.Contains(region.Name))));
                    return region;
                }
                return world.Find(attack.TargetRegion);
            }
            // a defence doesn't need a region
            return null;            
        }
    }
}
