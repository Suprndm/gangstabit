using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gangstabit.Business.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Bet> Bets { get; set; }
        public double TotalWage { get; set; }
        public double GamesCount { get; set; }
        public double TotalEarn { get; set; }
        public double WinRate { get; set; }
        public double AverageTarget { get; set; }
        public double AverageWage { get; set; }

        public void ComputeStats()
        {
            TotalWage = 0;
            GamesCount = Bets.Count;
            TotalEarn = 0;
            WinRate = 0;
            AverageTarget = 0;
            AverageWage = 0;

            TotalWage = Bets.Sum(bet => bet.Wage);

            var wonBets = Bets.Where(bet => bet.Target <= bet.Game.Multiplier).ToList();
            var lostBets = Bets.Where(bet => bet.Target > bet.Game.Multiplier).ToList();

            TotalEarn = wonBets.Sum(bet => bet.Target * bet.Wage) - lostBets.Sum(bet => bet.Wage);
            WinRate = (double) wonBets.Count / Bets.Count;
            AverageTarget = Bets.Sum(bet => bet.Target) / GamesCount;
            AverageWage = Bets.Sum(bet => bet.Wage) / GamesCount;
        }
    }
}
