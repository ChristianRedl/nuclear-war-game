using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Entities
{
    public abstract class Action
    {
        public event EventHandler TargetReached;
        public string Name { get; set; }
        public List<string> News { get; set; }
        public int Quantity { get; set; }
        public virtual void OnTargetReached(TargetReachedEventArgs e)
        {
            TargetReached?.Invoke(this, e);
        }
    }

    public class TargetReachedEventArgs : EventArgs
    {
        public Region Region { get; set; }
    }

    public abstract class AttackAction : Action
    {
        public string TargetRegion { get; set; }
        public long Destruction { get; set; }
        public string Location { get; set; }
    }

    public class Attack : AttackAction
    {
        public long DeploymentTimeSec { get; set; }
        public List<string> Announcement { get; set; }
    }

    public class Defence : Action
    {
        public double Reliability { get; set; }
        public double Effectiveness { get; set; }
        public bool Active { get; set; }
    }

    public class Random : AttackAction
    {

    }

}
