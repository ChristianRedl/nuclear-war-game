using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Entities
{
    public class GameSet
    {
        public List<Introduction> Introductions { get; set; }
        public World World { get; set; }
        public List<Nation> Nations { get; set; }
        public List<Random> RandomEvents { get; set; }
    }
}
