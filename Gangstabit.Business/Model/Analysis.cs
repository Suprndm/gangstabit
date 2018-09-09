using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Gangstabit.Business.Model
{
    public class Analysis
    {
        [JsonIgnore]
        public IList<Player> Players { get; set; }

        [JsonIgnore]
        public IList<Game> Games { get; set; }

        public double TotalPlayerEarn { get; set; }
        public double TotalPlayerLost { get; set; }
        public double TotalWage { get; set; }

        public double TotalRoi { get; set; }    

        public void ComputeStats()
        {
            TotalPlayerEarn = Games.Sum(game => game.TotalPlayerEarn);
            TotalPlayerLost = Games.Sum(game => game.TotalPlayerLost);
            TotalWage = Games.Sum(game => game.TotalWage);

            TotalRoi = Games.Sum(g => g.GameRoi) / Games.Count;
        }
    }
}
