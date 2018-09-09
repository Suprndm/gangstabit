using System;
using System.Collections.Generic;
using System.Text;

namespace Gangstabit.Simulator
{
    public class ControllerSettings
    {
        public double BaseBet { get; set; }
        public double Target { get; set; }
        public double Multiplier { get; set; }
        public int PassGames { get; set; }
        public double Reducer { get; set; }
    }
}
