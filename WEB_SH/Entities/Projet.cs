using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_SH.Entities
{
    public class Projet : Base
    {
        [Required]
        [StringLength(200)]
        public string Titre { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Url]
        public string Lien { get; set; } = string.Empty;

        [Required]
        public int PersonneId { get; set; }

        [ForeignKey("PersonneId")]
        public virtual Personne Personne { get; set; }
    }
}