using System.Threading.Tasks;
using Gangstabit.Business.Model;
using Gangstabit.Business.Ports;
using Microsoft.EntityFrameworkCore;
using DbGame = Gangstabit.DataAccess.Model.Game;
using DbPlayer = Gangstabit.DataAccess.Model.Player;
using DbBet = Gangstabit.DataAccess.Model.Bet;
namespace Gangstabit.DataAccess
{
    public class GameRepository :IGameRepository
    {
        public async Task SaveGameAsync(Game game)
        {
            using (var context = new GangstabitContext())
            {
                var existingGame = await context.Games.FirstOrDefaultAsync(g => g.EndDate == game.EndDate).ConfigureAwait(false);

                if (existingGame != null)
                    return;

                var dbGame = new DbGame()
                {
                    EndDate = game.EndDate,
                    Multiplier = game.Multiplier
                };

                foreach (var gameBet in game.Bets)
                {
                    var dbPlayer = await context.Players.FirstOrDefaultAsync(p => p.Name == gameBet.Player.Name).ConfigureAwait(false);

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

    }
}
