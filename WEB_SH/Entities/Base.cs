using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_SH.Entities
{
    public class Base
    {
        public Base()
        {
            DateCreation = DateTime.UtcNow;
            CreePar = 0;
            EstSupprime = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public int CreePar { get; set; }

     
        public bool? EstSupprime { get; set; }
        public DateTime? SupprimeA { get; set; }
        public int? SupprimePar { get; set; }
    }
}
