using SimpleNuclearWarGameplayDemo.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Data
{
    public static class SampleData
    {
        public static List<string> Single(string html)
        {
            return new List<string> { html };
        }

        public static GameSet GenerateDemoData()
        {
            GameSet gameSet = new GameSet();

            gameSet.World = new World();
            gameSet.World.Regions = new List<Region>();
            gameSet.World.Regions.Add(new Region { Name = "Russia", Population = 143000000 });
            gameSet.World.Regions.Add(new Region { Name = "USA",    Population = 320000000 });
            gameSet.World.Regions.Add(new Region { Name = "China", Population = 1300000000 });
            gameSet.World.Regions.Add(new Region { Name = "England", Population = 50000000 });
            gameSet.World.Regions.Add(new Region { Name = "Germany", Population = 80000000 });
            gameSet.World.Regions.Add(new Region { Name = "Scotland", Population = 5000000,
                News = Single("<img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/7/78/Highland_Cattle_4.jpg/250px-Highland_Cattle_4.jpg\"/><br/>The Highland cattle goes extinct.") });
            gameSet.World.Regions.Add(new Region { Name = "Switzerland", Population = 8000000,
                News = new List<string> { "The peaceful Switzerland has been annihilate." } });

            gameSet.Introductions = new List<Introduction>();
            gameSet.Introductions.Add(new Introduction
            {
                News = "<blockquote class=\"twitter-tweet\" data-lang=\"en\"><p lang=\"en\" dir=\"ltr\">Did China ask us if it was OK to devalue their currency (making it hard for our companies to compete), heavily tax our products going into..</p>&mdash; Donald J. Trump (@realDonaldTrump) <a href=\"https://twitter.com/realDonaldTrump/status/805538149157969924\">December 4, 2016</a></blockquote><br/><br/>Trump subsequently declared war on China!"
            });

            gameSet.Nations = new List<Nation>();
            gameSet.Nations.Add(new Nation
            {
                Name = "The English Empire",
                Ruler = "Theresa May",
                Description = "A fromer empire frozen in time.",
                Regions = new List<string> { "England", "Scotland" },
                Playable = true,
                ActionLimit = 4,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Trident",
                        Quantity = 5,
                        Location = "Scotland",
                        DeploymentTimeSec = 120,
                        Announcement = new List<string>
                        {
                            "England is launching one of it's <a href=\"https://en.wikipedia.org/wiki/Vanguard-class_submarine\">Vanguard Class</a> submarines."
                        },                        
                        Destruction = 2000000,
                        News = new List<string>
                        {
                            "England nuked {target}, {deaths} killed by the initial blast. {target_ruler} sworn retribution.",
                            "Special jam delivery for {target}"
                        }                        
                    },
                    new Attack
                    {
                        Name = "Brexit",
                        Quantity = 1,
                        Destruction = -25000,
                        DeploymentTimeSec = 45,
                        Announcement = new List<string>
                        {
                            "<b>Brexit means Brexit</b>"
                        },
                        TargetRegion = "Germany",
                        News = new List<string>
                        {
                            "Pound Sterling fell to under one US Dollar. Banks relocated to Frankfurt."
                        }
                    }
                },
                Defences = new List<Defence>
                {
                    new Defence
                    {
                        Name = "Radar",
                        Quantity = 5,
                        Reliability = 10,
                        Effectiveness = 50,
                        News = new List<string>
                        {
                            "England started to feed lots of carrots to their marksmen. Who shot down the attack."
                        },
                        Active = false
                    }
                },
                WinningSpeech = new List<string>
                {
                    "<p>Rule Britannia!</p>",
                    "Bringing innovative British jam to the world."
                },
                LoosingSpeech = new List<string>
                {
                    "England is buggered."
                }                
            });
            gameSet.Nations.Add(new Nation
            {
                Name = "Russia",
                Ruler = "Vladimir Putin",
                Description = "",
                Regions = new List<string> { "Russia" },
                Playable = true,
                ActionLimit = 10,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "ICBM R-7",
                        Quantity = 50,
                        DeploymentTimeSec = 10,
                        Destruction = 5000000,
                        News = new List<string>
                        {
                            "Russia nuked {target}, {deaths} killed by the initial blast. {target_ruler} is in hiding and could not be found for a statement.",
                            "R-7 Semyorka decending over {target} from Russia with love."
                        }
                    },
                    new Attack
                    {
                        Name = "RT-2PM2 Topol-M",
                        Quantity = 10,
                        Destruction = 2500000,
                        DeploymentTimeSec = 45,
                        Announcement = new List<string>
                        {
                            "Russia is moving mobile launch platforms into positions along it's border. <img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/19-03-2012-Parade-rehearsal_-_Topol-M.jpg/800px-19-03-2012-Parade-rehearsal_-_Topol-M.jpg\"/>."
                        },
                        News = new List<string>
                        {
                            "ракета твердотопливная улучшенные тактико-технические характеристики",
                            "{deaths} died in {target}."
                        }
                    }
                },
                Defences = new List<Defence>
                {
                    new Defence
                    {
                        Name = "Radar",
                        Quantity = 1,
                        Reliability = 10,
                        Effectiveness = 50,
                        News = new List<string>
                        {
                            "Russian Defences."
                        },
                        Active = false
                    }
                },
                WinningSpeech = new List<string>
                {
                    "Putin happy."
                },
                LoosingSpeech = new List<string>
                {
                    "Прощай"
                }
            });
            gameSet.Nations.Add(new Nation
            {
                Name = "America First",
                Ruler = "Donald J Trump",
                Description = "",
                Regions = new List<string> { "USA" },
                Playable = true,
                ActionLimit = 15,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "Intercontinental Hydrogen",
                        Quantity = 50,
                        DeploymentTimeSec = 15,
                        Destruction = 5000000,
                        News = new List<string>
                        {
                            "Trump nuked {target}, {deaths} killed by the initial blast. {target_ruler} is confused about the unprovoked attack.",
                            "The light of a thousand suns decented on {target}. Killing {deaths} souls in an instance."
                        }
                    },
                                        new Attack
                    {
                        Name = "Twitter",
                        Quantity = 1000,
                        DeploymentTimeSec = 1,
                        Destruction = 1,
                        News = new List<string>
                        {
                            "<b>Donald J. Trump</b> @realDonaldTrump {time}:<br/><p>{target_ruler} is an idiot. Just look at his hands, tiny! Total disaster!</p>",
                            "<b>Donald J. Trump</b> @realDonaldTrump {time}:<br/><p>See you in court {target_ruler}!</p>",
                            "<b>Donald J. Trump</b> @realDonaldTrump {time}:<br/><p>Look at the poor people in {target}! They let some bad people in. So sad!</p>",
                        }
                    },
                },
                Defences = new List<Defence>
                {
                    new Defence
                    {
                        Name = "Star Wars",
                        Quantity = 1,
                        Reliability = 10,
                        Effectiveness = 50,
                        News = new List<string>
                        {
                            "Laser in the sky."
                        },
                        Active = false
                    },
                    new Defence
                    {
                        Name = "NSA Cyber Defence",
                        Quantity = 10,
                        Reliability = 5,
                        Effectiveness = 100,
                        News = new List<string>
                        {
                            "NSA intercepted some hackers. All is good."
                        },
                        Active = false
                    }
                },
                WinningSpeech = new List<string>
                {
                    "<p>It is gonna be Great!</p>"
                },
                LoosingSpeech = new List<string>
                {
                    "It was supposed to be great. Sad!"
                }
            });
            gameSet.Nations.Add(new Nation
            {
                Name = "China",
                Ruler = "Xi Jinping",
                Description = "",
                Regions = new List<string> { "China" },
                Playable = true,
                ActionLimit = 12,
                Attacks = new List<Attack>
                {
                    new Attack
                    {
                        Name = "DF-41",
                        Quantity = 50,
                        DeploymentTimeSec = 7,
                        Destruction = 2500000,
                        News = new List<string>
                        {
                            "China attacked {target}, {deaths} killed."
                        }
                    },
                },
                Defences = new List<Defence>
                {
                    
                },
                WinningSpeech = new List<string>
                {
                    "<p>All are equal under the red flag!</p>"
                },
                LoosingSpeech = new List<string>
                {
                    "You might have laid waste to China, but Kommunism will never die!"
                }
            });

            gameSet.RandomEvents = new List<Entities.Random>();
            gameSet.RandomEvents.Add(new Entities.Random
            {
                Name = "Terror Attack",
                Quantity = 20,
                Destruction = 50,
                News = new List<string>
                {
                    "Terror Attack in {target} resulted in {deaths} deaths.",
                    "IS beheaded {deaths} infidels in {target}."
                }
            });
            gameSet.RandomEvents.Add(new Entities.Random
            {
                Name = "Hack",
                Quantity = 10,
                Destruction = 500,
                News = new List<string>
                {
                    "Hackers turned off the central heating in {target}. {deaths} people froze to death.",
                    "Hackers crashed the stock markets. {deaths} people lost their homes in {target} and got evicted."
                }
            });
            gameSet.RandomEvents.Add(new Entities.Random
            {
                Name = "Nature",
                Quantity = 10,
                Destruction = 10000,
                News = new List<string>
                {
                    "{deaths} killed in an earthquake in {target}."
                }
            });
            gameSet.RandomEvents.Add(new Entities.Random
            {
                Name = "Industrial Accidents",
                Quantity = 10,
                Destruction = 6000,
                News = new List<string>
                {
                    "<b>Fracking accident in {target}.</b><br/>As reports now indicate toxic chemicals were released into the groundwater. So far {deaths} deaths were linked to the incident.",
                    "<a href=\"http://www.forbes.com/sites/christopherhelman/2013/01/23/fracking-for-uranium/\">{target} started fracking for uranium.</a>",
                    "<b>Plane Crash.</b><br/>On {time}, shortly after takeoff a 787 Dreamliner crashed into a city in {target}. So far {deaths} bodies have been recovered.",
                    "<b>Oil Spill in {target}.</b><br/>BP issued a statement stating that they are very sorry about it.",
                }
            });
            gameSet.RandomEvents.Add(new Entities.Random
            {
                Name = "Migration",
                Quantity = 10,
                Destruction = -100000,
                News = new List<string>
                {
                    "{deaths} refugees arrived in {target}."
                }
            });

            return gameSet;
        }
    }
}
