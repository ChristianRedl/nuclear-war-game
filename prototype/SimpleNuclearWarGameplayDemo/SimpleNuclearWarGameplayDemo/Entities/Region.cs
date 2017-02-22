
using System.Collections.Generic;

namespace SimpleNuclearWarGameplayDemo.Entities
{
    public class Region
    {
        public string Name { get; set; }
        public long Population { get; set; }
        public List<string> News { get; set; }
    }
}