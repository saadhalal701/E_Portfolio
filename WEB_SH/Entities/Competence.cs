using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEB_SH.Shered;

namespace WEB_SH.Entities
{
    [Table("Competence")]
    public class Competence : Base
    {
        [Required]
        public int PersonneId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public NiveauCompetence Niveau { get; set; }

        [ForeignKey("PersonneId")]
        public virtual Personne Personne { get; set; }
    }
}