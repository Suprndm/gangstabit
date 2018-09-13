using System;
using System.Collections.Generic;
using System.Text;
using Gangstabit.Business.Controllers;
using Gangstabit.Business.Model;

namespace Gangstabit.Simulator
{
    public class SimulationResult
    {
        public SimulationResult(
            ControllerSettings controllerSettings,
            ControllerResults controllerResults,
            Player player,
            int initialWallet)
        {
            ControllerSettings = controllerSettings;
            TotalLoss = player.TotalLost;
            TotalWage = player.TotalWage;
            TotalWin = player.TotalEarn;
            Benefits = player.Benefits;
            WinRate = player.WinRate;
            Roi = player.Roi;
            GamesPlayed = player.GamesCount;
            InitialWallet = initialWallet;
            FinalWallet = player.Wallet;

            MaxConsecutiveLoss = controllerResults.MaxConsecutiveLoss;
            MaxCashLoss = controllerResults.MaxCashLoss;
            MaxWage = controllerResults.MaxWage;
            MaxWallet = controllerResults.MaxWallet;
            MaxCashLoss = controllerResults.MaxCashLoss;
            MinWallet = controllerResults.MinWallet;

            ComputeScores();
        }


        public ControllerSettings ControllerSettings { get; }

        public double TotalScore { get; private set; }
        public double BenefitsScore { get; private set; }
        public double RiskScore { get; private set; }

        public double TotalWage { get; }
        public double TotalLoss { get; }
        public double TotalWin { get; }
        public double Benefits { get; }
        public double WinRate { get; }
        public double Roi { get; }
        public int GamesPlayed { get; }

        public double InitialWallet { get; set; }
        public double FinalWallet { get; set; }

        public int MaxConsecutiveLoss { get; }
        public double MaxCashLoss { get; }
        public double MaxWage { get; }
        public double MaxWallet { get; }
        public double MinWallet { get; }

        private void ComputeScores()
        {
            BenefitsScore = (FinalWallet - InitialWallet) / (4 * (InitialWallet));
            BenefitsScore = Math.Min(1, BenefitsScore);

            RiskScore = (InitialWallet - (MaxCashLoss + MaxWage)  -(InitialWallet - MinWallet) )/ InitialWallet;

            TotalScore = (BenefitsScore + RiskScore) / 2 - Math.Abs(BenefitsScore - RiskScore) / 4;
        }

        public override string ToString()
        {
            return $"TotalScore : {TotalScore} (BScore: {BenefitsScore}, RScore: {RiskScore}";
        }
    }
}
