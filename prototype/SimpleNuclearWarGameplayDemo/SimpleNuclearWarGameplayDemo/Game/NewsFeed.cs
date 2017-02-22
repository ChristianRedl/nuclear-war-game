using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Game
{
    /// <summary>
    /// Interface from the Controller to the View that displays the News items as they 
    /// </summary>
    public interface NewsFeed
    {
        void DisplayHtml(string html);
    }
}
