using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gangstabit.Business.Model
{
    public class Game
    {
        public int Id { get; set; }

        public DateTime EndDate { get; set; }
        public double Multiplier { get; set; }

        public IList<Bet> Bets { get; set; }

        public double TotalWage { get; set; }
        public double TotalPlayerEarn { get; set; }


        public void ComputeStats()
        {
            TotalWage = 0;
            TotalPlayerEarn = 0;

            TotalWage = Bets.Sum(bet => bet.Wage);

            var wonBets = Bets.Where(bet => bet.Target <= bet.Game.Multiplier).ToList();
            var lostBets = Bets.Where(bet => bet.Target > bet.Game.Multiplier).ToList();

            TotalPlayerEarn = wonBets.Sum(bet => bet.Target * bet.Wage) - lostBets.Sum(bet => bet.Wage);
        }
    }
}
