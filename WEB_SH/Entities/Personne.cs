using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_SH.Entities
{
    public class Personne : Base
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [StringLength(500)]
        [Column("AdresseMail")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string MotDePasse { get; set; } = string.Empty;

        public string? Photo { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide")]
        [Column("NumeroTel")]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Le rôle est requis")]
        public string Role { get; set; } = string.Empty;

        public virtual ICollection<Competence> Competences { get; set; } = new List<Competence>();
        public virtual ICollection<Projet> Projets { get; set; } = new List<Projet>();
        public virtual ICollection<Message> MessagesEnvoyes { get; set; } = new List<Message>();
        public virtual ICollection<Message> MessagesRecus { get; set; } = new List<Message>();
    }
}