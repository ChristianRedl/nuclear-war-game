using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Entities
{
    public class World
    {
        public List<Region> Regions { get; set; }

        /// <summary>
        /// Find a region of the World by name. 
        /// Will only return the region if the population of the Region is greater 0.
        /// </summary>
        /// <param name="targetRegion">Name of the region.</param>
        /// <returns>The found Region or null.</returns>
        internal Region Find(string targetRegion)
        {
            if (targetRegion == null)
            {
                return null;
            }
            foreach(Region region in Regions)
            {
                if (region.Population > 0 && region.Name.ToLower().Equals(targetRegion.ToLower())) {
                    return region;
                }
            }
            return null;
        }

        internal long GetTotalPopulation()
        {
            long population = 0;
            foreach(Region r in Regions)
            {
                population += r.Population;
            }
            return population;
        }
    }
}
