﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Controllers
{
    public class RouletteController : IController
    {
        private readonly Player _player;
        private readonly double _baseBet;
        private readonly double _target;
        private double _currentBet;
        private double _multiplier;

        private int _maxConsecutiveLossCount;
        private int _consecutiveLossCount;
        private double _consecutiveCashLoss;
        private double _maxConsecutiveCashLoss;
        private double _maxWage;
        private double _minWallet;
        private readonly int _passGames;
        private int _currentPassedGame;
        private double _minBalance;
        private double _maxBalance;
        private double _maxWallet;
        private readonly double _reducer;

        public RouletteController(Player player, double baseBet, double target, double multiplier, int passGames, double reducer)
        {
            _player = player;
            _baseBet = baseBet;
            _target = target;
            _multiplier = multiplier;
            _passGames = passGames;
            _reducer = reducer;
            _currentPassedGame = _passGames;
            _currentBet = _baseBet;
            _maxConsecutiveLossCount = 0;
            _consecutiveLossCount = 0;
            _maxWage = 0;
            _minWallet = player.Wallet;
            _maxWallet = player.Wallet;

            _maxConsecutiveCashLoss = 0;
            _consecutiveCashLoss = 0;
        }

        private double ComputePreviousGamesBalance(IList<Game> previousGames, int range)
        {
            if (previousGames.Count < range) return 0;

            double balance = 0;

            var selectedGames = previousGames.TakeLast(range);

            foreach (var previousGame in selectedGames)
            {
                balance += previousGame.TotalPlayerEarn - previousGame.TotalPlayerLost;
            }

            return balance;
        }

        public Decision Play(IList<Game> previousGames)
        {

            bool passGame = StaticRandom.Rand(10) == 0;
            var balance = ComputePreviousGamesBalance(previousGames, 100);
            _minBalance = Math.Min(_minBalance, balance);
            _maxBalance = Math.Max(_maxBalance, balance);
            var wage = Math.Min(_baseBet, _player.Wallet);

            if (_player.Bets.Any())
            {
                var previousBet = _player.Bets.Last();
                if (previousBet.Wage > 0 && previousBet.IsWon == false)
                {
                    var modifier = balance / 30000000;
                    var multplier = (_multiplier - _reducer  * _consecutiveLossCount * _consecutiveLossCount * _consecutiveLossCount);
                    //multplier = multplier + modifier;
                    if (multplier < 0.75)
                    {
                        multplier = 0.75;
                    }

                    _currentBet = _currentBet * multplier ;
                    _consecutiveLossCount++;
                    _consecutiveCashLoss += previousBet.Wage;

                    //if (balance > 1000000)
                    //{
                    //    _currentPassedGame = 0;
                    //}

                    //if (_consecutiveLossCount == 10)
                    //{
                    //    _currentPassedGame =2;
                    //    _currentBet = _baseBet;
                    //    _consecutiveLossCount = 0;
                    //    _consecutiveCashLoss = 0;
                    //}

                }
                else if (previousBet.Wage > 0)
                {
                    _currentPassedGame = 0;
                    _currentBet = _baseBet;
                    _consecutiveLossCount = 0;
                    _consecutiveCashLoss = 0;
                }
                else
                {
                    _consecutiveLossCount = 0;
                }

                wage = _currentBet;
            }


            if (_currentPassedGame < _passGames)
            {
                _currentPassedGame++;
                wage = 0;
            }

            _maxWage = Math.Max(wage, _maxWage);
            _maxConsecutiveLossCount = Math.Max(_maxConsecutiveLossCount, _consecutiveLossCount);

            _minWallet = Math.Min(_minWallet, _player.Wallet);
            _maxWallet = Math.Max(_maxWallet, _player.Wallet);

            _maxConsecutiveCashLoss = Math.Max(_maxConsecutiveCashLoss, _consecutiveCashLoss);


            return new Decision()
            {
                Target = _target,
                Wage = wage
            };
        }

        public ControllerResults GetControllerResults()
        {
            return new ControllerResults()
            {
                MaxCashLoss = _maxConsecutiveCashLoss,
                MaxConsecutiveLoss = _maxConsecutiveLossCount,
                MaxWage = _maxWage,
                MaxWallet = _maxWallet,
                MinWallet = _minWallet,
            };
        }

        public override string ToString()
        {
            return $"MaxWage: {_maxWage}\n ConsecutiveLoss : {_maxConsecutiveLossCount} \n minWallet: {_minWallet} \n maxWallet : {_maxWallet}" +
                   $"\nMaxCashLoss {_maxConsecutiveCashLoss} balance [{_minBalance}, {_maxBalance}]";
        }

        public void Dispose()
        {
        }
    }
}
