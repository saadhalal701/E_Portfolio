using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WEB_SH.Models
{
    public class FormulaireCreationViewModel
    {
        [Required]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string? MotDePasse { get; set; }

        public IFormFile? Photo { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide")]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; }  

        // Propriété pour compatibilité 
        public string? NumeroTel
        {
            get => Telephone;
            set => Telephone = value;
        }
    }
}