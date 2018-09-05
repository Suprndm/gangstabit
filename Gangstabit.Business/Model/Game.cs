using System;
using System.Collections.Generic;
using System.Text;

namespace Gangstabit.Business.Model
{
    public class Game
    {
        public DateTime EndDate { get; set; }
        public double Multiplier { get; set; }

        public IList<Bet> Bets { get; set; }
    }
}
