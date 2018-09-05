using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Gangstabit.DataAccess.Model
{
    [Table("Bet", Schema = "Gangstabit")]
    public class Bet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }
        public Game Game { get; set; }

        public double Target { get; set; }
        public double Wage { get; set; }
    }
}
