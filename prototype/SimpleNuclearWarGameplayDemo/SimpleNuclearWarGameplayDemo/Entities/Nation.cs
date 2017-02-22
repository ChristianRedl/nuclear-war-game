using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Entities
{
    public class Nation
    {
        public string Name { get; set; }
        public string Ruler { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public List<string> Regions { get; set; }
        public int ActionLimit { get; set; }
        public bool Playable { get; set; }
        public List<Attack> Attacks { get; set; }
        public List<Defence> Defences { get; set; }
        public List<string> WinningSpeech { get; set; }
        public List<string> LoosingSpeech { get; set; }
    }
}
