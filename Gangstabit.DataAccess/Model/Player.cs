using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Gangstabit.DataAccess.Model
{
    [Table("Player", Schema = "Gangstabit")]
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [StringLength(25)]
        public string Name { get; set; }
    }
}
