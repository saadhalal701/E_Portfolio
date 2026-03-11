using System.Collections.Generic;
using WEB_SH.Entities;

namespace WEB_SH.Models
{
    public class ProfilModel
    {
        public Personne Personne { get; set; } = new Personne();
        public List<Competence> Competences { get; set; } = new List<Competence>();
        public List<Projet> Projets { get; set; } = new List<Projet>();
    }
}
