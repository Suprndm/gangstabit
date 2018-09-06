using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gangstabit.Business.Model
{
    public class Analysis
    {
        public IList<Player> Players { get; set; }
        public IList<Game> Games { get; set; }

        public double TotalPlayerEarn { get; set; }
        public double TotalWage { get; set; }

        public void ComputeStats()
        {
            TotalPlayerEarn = Games.Sum(game => game.TotalPlayerEarn);
            TotalWage = Games.Sum(game => game.TotalWage);
        }
    }
}
