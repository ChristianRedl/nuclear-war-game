using SimpleNuclearWarGameplayDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Game
{
    /// <summary>
    /// Used to control game moves and their impact throught the board
    /// </summary>
    public class GameControl
    {
        private static System.Random random = new System.Random();
        private List<AIPlayer> ais = new List<AIPlayer>();
        private Terrorist terrorAi;
        public HumanPlayer Player { get; set; }

        List<Introduction> Introductions { get; set; }
        World World { get; set; }
        List<Nation> NonPlayerNations { get; set; }        
        List<Entities.Random> RandomEvents { get; set; }
        

        public GameControl(GameSet board)
        {
            World = board.World;
            NonPlayerNations = board.Nations;
            Introductions = board.Introductions;
            RandomEvents = board.RandomEvents;
        }

        public List<Nation> GetPlayableNations()
        {
            var plableNations = new List<Nation>();
            foreach(Nation nation in NonPlayerNations)
            {
                if(nation.Playable)
                {
                    plableNations.Add(nation);
                }
            }
            return plableNations;
        }

        internal void StartAIs(EventHandler targetHit, NewsFeed news)
        {
            foreach(Nation enemy in NonPlayerNations)
            {
                ais.Add(new AIPlayer(enemy, World, targetHit, news));
            }
            terrorAi = new Terrorist(RandomEvents,targetHit,World);
        }

        internal string GetIntroduction()
        {
            return GetRandom(Introductions).News;
        }

        public void SelectPlayerNation(Nation nation)
        {
            NonPlayerNations.Remove(nation);
            Player = new HumanPlayer(nation);
        }

        public static T GetRandom<T>(List<T> choice)
        {
            if (choice == null || choice.Count < 1)
            {
                return default(T);
            }
            return choice[random.Next(choice.Count)];
        }

        public static System.Random GetRandom()
        {
            return random;
        }


        public void Hit(Region region, AttackAction attack, NewsFeed news)
        {
            if (region == null || region.Population < 1)
            {
                // this attack has nothing to hit? 
                return;
            }

            Player targetPlayer = FindPlayer(GetNation(region));
            // before calculation the hit, lets check it the player has any active defences that might help
            Defence usableDefence = null;
            if(targetPlayer!=null) { 
                foreach (Defence defence in targetPlayer.GetActiveDefences())
                {
                    if (defence.Reliability > random.Next(100))
                    {
                        usableDefence = defence;
                        targetPlayer.GetActiveDefences().Remove(usableDefence);
                        break;
                    }
                }
            }

            double deaths = (double)attack.Destruction * random.NextDouble();
            if(usableDefence!= null)
            {
                deaths = (deaths / 100) * usableDefence.Effectiveness;
            }

            region.Population = region.Population > deaths ? region.Population - (long)deaths : 0;

            string newsItem = GetRandom(attack.News);
            if (usableDefence?.News != null && usableDefence.News.Count > 0)
            {
                newsItem = GetRandom(usableDefence.News);
            }
            newsItem = newsItem
                .Replace("{deaths}", ((long)Math.Abs(deaths)).ToString())
                .Replace("{target}", region?.Name)
                .Replace("{target_ruler}", targetPlayer == null ? "Government" : targetPlayer?.GetNation().Ruler)
                .Replace("{target_ruler_img}", targetPlayer == null ? "" : targetPlayer?.GetNation().Picture)
                .Replace("{time}", DateTime.Now.ToString());

            news.DisplayHtml(newsItem);

            if(region.Population <= 0)
            {
                news.DisplayHtml(GetRandom(region.News));
                // if a region has been destroyed all attacks stationed in this this region should be removed
                Nation nation = GetNation(region);
                // the region might not be part of a nation
                if (nation!=null)
                {
                    foreach (Attack a in nation.Attacks)
                    {
                        if (region.Name.Equals(a.Location))
                        {
                            a.Quantity = 0;
                        }
                    }

                    // this might have been the last region of this nation and the player is destroyed
                    long totalPopulation = 0;
                    foreach (string regName in nation.Regions)
                    {
                        Region natRegion = World.Find(regName);
                        if (natRegion != null)
                        {
                            totalPopulation += natRegion.Population;
                        }
                    }
                    if (totalPopulation < 1)
                    {
                        nation.Attacks.Clear();
                        nation.Defences.Clear();
                        news.DisplayHtml(GetRandom(nation.LoosingSpeech));
                    }
                }
            }

            // this attack has finished
            Player attacker = FindPlayerFor(attack);
            attacker?.GetDeployedAttacks().Remove((Attack)attack);
        }

        private Player FindPlayerFor(Entities.Action attack)
        {
            foreach(Player p in GetAllPlayers())
            {
                if(p.Owns(attack))
                {
                    return p;
                }
            }
            return null;
        }

        private IEnumerable<Player> GetAllPlayers()
        {
            List<Player> allPlayers = new List<Player>();
            allPlayers.Add(Player);
            allPlayers.AddRange(ais);
            return allPlayers;
        }

        private Player FindPlayer(Nation targetNation)
        {
            if(Player.GetNation().Equals(targetNation))
            {
                return Player;
            }
            foreach(Player p in ais)
            {
                if (p.GetNation().Equals(targetNation))
                {
                    return p;
                }
            }
            return null;
        }

        /// <summary>
        /// Evaluates the current game, for example if any users have died
        /// </summary>
        /// <param name="newsFeeed"></param>
        /// <returns>ture if the game is over</returns>
        public bool Refresh(NewsFeed newsFeeed)
        {
            // remove any players with population 0
            // evaluate if more than one player is left
            return false;
        }

        internal void StopAIs()
        {
            foreach(AIPlayer ai in ais)
            {
                ai.Stop();
            }
            lock(this)
            {
                if(terrorAi != null)
                {
                    terrorAi.Stop();
                    terrorAi = null;
                }                
            }            
        }

        internal Nation GetNation(string name)
        {
            if (Player?.GetNation() != null && Player.GetNation().Name.Equals(name))
            {
                return Player.GetNation();
            }
            foreach (Nation nation in NonPlayerNations)
            {
                if (nation.Name.Equals(name))
                {
                    return nation;
                }
            }
            return null;
        }

        internal Nation GetNation(Region region)
        {
            if (region == null)
            {
                return null;
            }
            if (Player?.GetNation() != null && Player.GetNation().Regions.Contains(region.Name))
            {
                return Player.GetNation();
            }
            foreach (Nation nation in NonPlayerNations)
            {                
                if (nation.Regions.Contains(region.Name))
                {
                    return nation;
                }
            }
            return null;
        }
    }
}
