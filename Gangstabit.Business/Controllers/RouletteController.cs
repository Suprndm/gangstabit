using System;
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
        private double _maxWage;
        private double _minWallet;
        private readonly int _passGames;
        private int _currentPassedGame;

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
        }

        public Decision Play(IList<Game> previousGames)
        {
          

            var wage = Math.Min(_baseBet, _player.Wallet);

            if (_player.Bets.Any())
            {
                var previousBet = _player.Bets.Last();
                if (previousBet.Wage>0 &&previousBet.IsWon == false)
                {
                    _currentBet = _currentBet * (_multiplier - _reducer * _consecutiveLossCount);
                    _consecutiveLossCount++;
                }
                else if(previousBet.Wage > 0)
                {
                    _currentPassedGame = 0;
                    _currentBet = _baseBet;
                    _maxConsecutiveLossCount = Math.Max(_maxConsecutiveLossCount, _consecutiveLossCount);
                    _consecutiveLossCount = 0;
                }

                wage = _currentBet;
            }

            _maxWage = Math.Max(wage, _maxWage);

            _minWallet = Math.Min(_minWallet, _player.Wallet);
            _maxWallet = Math.Max(_maxWallet, _player.Wallet);

            if (_currentPassedGame < _passGames)
            {
                _currentPassedGame++;
                wage = 0;
            }

            return new Decision()
            {
                Target = _target,
                Wage = wage
            };
        }

        public override string ToString()
        {
            return $"MaxWage: {_maxWage}\n ConsecutiveLoss : {_maxConsecutiveLossCount} \n minWallet: {_minWallet} \n maxWallet : {_maxWallet}";
        }

        public void Dispose()
        {
        }
    }
}
