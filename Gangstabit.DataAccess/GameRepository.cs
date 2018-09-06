using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gangstabit.Business.Model;
using Gangstabit.Business.Ports;
using Microsoft.EntityFrameworkCore;
using DbGame = Gangstabit.DataAccess.Model.Game;
using DbPlayer = Gangstabit.DataAccess.Model.Player;
using DbBet = Gangstabit.DataAccess.Model.Bet;

namespace Gangstabit.DataAccess
{
    public class GameRepository : IGameRepository
    {
        public async Task SaveGameAsync(Game game)
        {
            using (var context = new GangstabitContext())
            {
                var existingGame = await context.Games.FirstOrDefaultAsync(g => g.EndDate == game.EndDate)
                    .ConfigureAwait(false);

                if (existingGame != null)
                    return;

                var dbGame = new DbGame()
                {
                    EndDate = game.EndDate,
                    Multiplier = game.Multiplier
                };

                foreach (var gameBet in game.Bets)
                {
                    var dbPlayer = await context.Players.FirstOrDefaultAsync(p => p.Name == gameBet.Player.Name)
                        .ConfigureAwait(false);

                    if (dbPlayer == null)
                    {
                        dbPlayer = new DbPlayer
                        {
                            Name = gameBet.Player.Name,
                        };

                        context.Players.Add(dbPlayer);
                    }

                    var dbBet = new DbBet
                    {
                        Game = dbGame,
                        Player = dbPlayer,
                        Target = gameBet.Target,
                        Wage = gameBet.Wage
                    };

                    context.Add(dbBet);
                }

                context.Add(dbGame);

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task<Analysis> GetAnalysisAsync()
        {
            IDictionary<int, Game> games = new Dictionary<int, Game>();
            IDictionary<int, Player> players = new Dictionary<int, Player>();

            using (var context = new GangstabitContext())
            {


                foreach (var dbBet in context.Bets.Include(b => b.Player).Include(b => b.Game))
                {
                    Player player = null;
                    Game game = null;

                    if (games.ContainsKey(dbBet.GameId))
                    {
                        game = games[dbBet.GameId];
                    }
                    else
                    {
                        game = new Game()
                        {
                            Id = dbBet.Game.Id,
                            Bets = new List<Bet>(),
                            EndDate = dbBet.Game.EndDate,
                            Multiplier = dbBet.Game.Multiplier
                        };

                        games.Add(game.Id, game);
                    }

                    if (players.ContainsKey(dbBet.PlayerId))
                    {
                        player = players[dbBet.PlayerId];
                    }
                    else
                    {
                        player = new Player()
                        {
                            Id = dbBet.Player.Id,
                            Name = dbBet.Player.Name,
                            Bets = new List<Bet>()
                        };

                        players.Add(player.Id, player);
                    }

                    var bet = new Bet()
                    {
                        Game = game,
                        Player = player,
                        Id = dbBet.Id,
                        Target = dbBet.Target,
                        Wage = dbBet.Wage
                    };

                    game.Bets.Add(bet);
                    player.Bets.Add(bet);
                }
            }

            var gamesList = games.Values.ToList();
            foreach (var game in gamesList)
            {
                game.ComputeStats();
            }

            var playersList = players.Values.ToList();
            foreach (var player in playersList)
            {
                player.ComputeStats();
            }

            var analysis = new Analysis()
            {
                Games = gamesList,
                Players = playersList,
            };

            analysis.ComputeStats();

            return Task.FromResult(analysis);
        }
    }
}
