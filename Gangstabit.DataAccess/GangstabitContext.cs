using Gangstabit.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Gangstabit.DataAccess
{
    public class GangstabitContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Back;Trusted_Connection=True;");
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
