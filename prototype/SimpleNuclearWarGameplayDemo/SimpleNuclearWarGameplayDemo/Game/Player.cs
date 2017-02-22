using SimpleNuclearWarGameplayDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SimpleNuclearWarGameplayDemo.Game
{
    public abstract class Player
    {
        Nation nation;
        List<Defence> activeDefences = new List<Defence>();
        List<Attack> deployedAttacks = new List<Attack>();

        public Player(Nation nation)
        {
            this.nation = nation;
        }

        internal Nation GetNation()
        {
            return nation;
        }

        internal List<Entities.Action> GetAvailableActions()
        {
            List<Entities.Action> allActions = new List<Entities.Action>();
            allActions.AddRange(nation.Attacks);
            allActions.AddRange(nation.Defences);
            return allActions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>union of deployedAttacks and activeDefences</returns>
        internal List<Entities.Action> GetActiveActions()
        {
            List<Entities.Action> activeActions = new List<Entities.Action>();
            activeActions.AddRange(deployedAttacks);
            activeActions.AddRange(activeDefences);
            return activeActions;
        }

        internal List<Defence> GetActiveDefences()
        {
            return activeDefences;
        }

        internal List<Attack> GetDeployedAttacks()
        {
            return deployedAttacks;
        }

        internal long Population(World world)
        {
            long population = 0;
            foreach(var regionName in nation.Regions)
            {
                var region = world.Find(regionName);
                if(region != null)
                {
                    population += region.Population;
                }
            }
            return population;
        }

        internal bool Trigger(Entities.Action action, Region region, NewsFeed newsFeed)
        {
            if(action.Quantity < 1 || nation.ActionLimit <= GetActiveActions().Count)
            {
                // reached limit of allowed actions for this nation
                return false;
            }

            if (action is Attack)
            {
                // AttackAction have a delay between triggering and taking effect.
                // Same object is potentially added to the list more often then once.
                deployedAttacks.Add((Attack)action);
                newsFeed?.DisplayHtml(GameControl.GetRandom(((Attack)action).Announcement));
                if (--action.Quantity < 1)
                {
                    // this was the last attack of this type the player had ...
                    nation.Attacks.Remove((Attack)action);
                }
                // Create Thread to wait until actin reaches it's targed
                Timer timer = new Timer();
                timer.Interval = ((Attack)action).DeploymentTimeSec * 1000;
                timer.Elapsed += (s, e) => {
                    ((Attack)action).OnTargetReached(new TargetReachedEventArgs
                    {
                        Region = region
                    });
                    timer.Stop();
                };
                timer.Start();
            }
            else if (action is Defence)
            {
                activeDefences.Add((Defence)action);
                if (--action.Quantity < 1)
                {
                    nation.Defences.Remove((Defence)action);
                }

            }            
            return true;
        }

        internal Entities.Action GetAvailableAction(string selectedItem)
        {
            foreach(Entities.Action action in GetAvailableActions())
            {
                if(action.Name.Equals(selectedItem))
                {
                    return action;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if the given action belongs to the player
        /// </summary>
        /// <param name="a">any defence or attack action</param>
        /// <returns>true if the player is responsible for this action</returns>
        internal bool Owns(Entities.Action a)
        {
            if(a is Defence)
            {
                return nation.Defences.Contains((Defence)a);
            }
            if(a is Attack)
            {
                return nation.Attacks.Contains((Attack)a);
            }
            return false;
        }
    }
}
