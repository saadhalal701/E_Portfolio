using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEB_SH.Entities;

public class Message : Base
{
    [Required]
    public int ExpediteurId { get; set; }

    [ForeignKey("ExpediteurId")]
    public virtual Personne Expediteur { get; set; } = null!; 

    [Required]
    public int DestinataireId { get; set; }

    [ForeignKey("DestinataireId")]
    public virtual Personne Destinataire { get; set; } = null!; 

    [Required]
    [StringLength(200)]
    public string Objet { get; set; } = string.Empty; 

    [Required]
    public string Contenu { get; set; } = string.Empty; 

    public DateTime DateEnvoi { get; set; } = DateTime.Now;

    public bool EstLu { get; set; } = false;
}