using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Gangstabit.Business.Model
{
    public class Player
    {
        public Player()
        {
            Bets = new List<Bet>();
        }

        public double Wallet { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public IList<Bet> Bets { get; set; }

        public double TotalWage { get; set; }
        public int GamesCount { get; set; }
        public double TotalEarn { get; set; }
        public double TotalLost { get; set; }

        public double WinRate { get; set; }
        public double AverageTarget { get; set; }
        public double AverageWage { get; set; }
        public double Roi { get; set; }
        public double Benefits { get; set; }
        public string ControllerResults { get; set; }

        public void ComputeStats()
        {
            TotalWage = 0;
            GamesCount = Bets.Count(b => b.Wage > 0);
            TotalEarn = 0;
            WinRate = 0;
            AverageTarget = 0;
            AverageWage = 0;

            TotalWage = Bets.Sum(bet => bet.Wage);

            var wonBets = Bets.Where(bet => bet.Target > 0 && bet.Target <= bet.Game.Multiplier).ToList();
            var lostBets = Bets.Where(bet => bet.Target == 0 || bet.Target > bet.Game.Multiplier).ToList();

            TotalEarn = wonBets.Sum(bet => (bet.Target - 1) * bet.Wage);

            TotalLost = lostBets.Sum(b => b.Wage);

            Roi = (TotalEarn - TotalLost) / TotalWage;

            WinRate = (double)wonBets.Count / Bets.Count;
            AverageTarget = Bets.Sum(bet => bet.Target) / wonBets.Count;
            AverageWage = Bets.Sum(bet => bet.Wage) / GamesCount;
            Benefits = TotalEarn - TotalLost;
        }

        public override string ToString()
        {
            return $"Wallet :{Wallet} \nBenefits :{Benefits}\nRoi : {Roi}\n played {GamesCount} games";
        }
    }
}
