using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Gangstabit.Business.Model
{
    public class Game
    {
        public int Id { get; set; }

        public DateTime EndDate { get; set; }
        public double Multiplier { get; set; }

        [JsonIgnore]
        public IList<Bet> Bets { get; set; }

        public double TotalWage { get; set; }
        public double TotalPlayerEarn { get; set; }
        public double TotalPlayerLost { get; set; }
        public double GameRoi { get; set; }


        public void ComputeStats()
        {
            TotalWage = 0;
            TotalPlayerEarn = 0;

            TotalWage = Bets.Sum(bet => bet.Wage);

            var wonBets = Bets.Where(bet => bet.Target > 0 && bet.Target <= bet.Game.Multiplier).ToList();
            var lostBets = Bets.Where(bet => bet.Target == 0 || bet.Target > bet.Game.Multiplier).ToList();

            TotalPlayerEarn = wonBets.Sum(bet => (bet.Target - 1) * bet.Wage);
            TotalPlayerLost = lostBets.Sum(b => b.Wage);

            GameRoi = (TotalPlayerEarn - TotalPlayerLost) / TotalWage;
        }
    }
}
