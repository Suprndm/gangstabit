using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Controllers
{
    public class SimpleController: IController
    {
        private readonly Player _player;
        private readonly int _baseBet;
        private readonly double _target;
        public SimpleController(Player player, int baseBet, double target)
        {
            _player = player;
            _baseBet = baseBet;
            _target = target;
        }

        public Decision Play(IList<Game> previousGames)
        {
            var wage = Math.Min(_baseBet, _player.Wallet);
            return new Decision()
            {
                Target = _target,
                Wage = wage
            };
        }

        public void Dispose()
        {
        }
    }
}
