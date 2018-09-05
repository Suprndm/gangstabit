using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Gangstabit.DataAccess.Model
{
    [Table("Game", Schema = "Gangstabit")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public double Multiplier { get; set; }
    }
}
