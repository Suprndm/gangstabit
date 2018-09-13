using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gangstabit.Business;
using Gangstabit.Business.Controllers;
using Gangstabit.Business.Model;
using Newtonsoft.Json;

namespace Gangstabit.Simulator
{
    public class SimulationRunner
    {
        public async Task<IList<Game>> LoadGames(DateTime? from = null, DateTime? to = null)
        {
            var logReader = new System.IO.StreamReader($@"C:\GangstabitResults\gamesJson.json");
            var json = await logReader.ReadToEndAsync();
            var games = JsonConvert.DeserializeObject<IList<Game>>(json);
            if (from != null)
            {
                games = games.Where(g => g.EndDate >= from.Value).ToList();
            }


            if (to != null)
            {
                games = games.Where(g => g.EndDate <= to.Value).ToList();
            }

            var lastGame = games.Last();

            return games;
        }

        public void StartWith(Player player, IController controller, IList<Game> games)
        {
            var previousGames = new List<Game>();
            foreach (var game in games)
            {
                    var decision = controller.Play(previousGames);
                    var bet = new Bet()
                    {
                        Game = game,
                        Player = player,
                        Target = decision.Target,
                        Wage = decision.Wage
                    };

                    player.Bets.Add(bet);

                    player.Wallet += -decision.Wage;
                    if (game.Multiplier >= decision.Target)
                    {
                        bet.IsWon = true;
                        player.Wallet += decision.Wage * decision.Target;
                    }


                previousGames.Add(game);

                if (player.Wallet <= 0)
                {
                    //Console.WriteLine("Dead");
                    break;
                }
            }
        }
    }
}
