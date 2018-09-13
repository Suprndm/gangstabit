using System;
using System.Collections.Generic;
using System.Text;

namespace Gangstabit.Business.Controllers
{
    public class ControllerResults
    {
        public int MaxConsecutiveLoss { get; set; }
        public double MaxCashLoss { get; set; }
        public double MaxWage { get; set; }
        public double MaxWallet { get; set; }
        public double MinWallet { get; set; }
    }
}
