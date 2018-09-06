using System;
using System.Collections.Generic;
using System.Text;

namespace Gangstabit.Business.Model
{
    public class Bet
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public Game Game { get; set; }
        public double Target { get; set; }
        public double Wage { get; set; }
    }
}
