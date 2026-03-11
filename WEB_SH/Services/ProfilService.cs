using System.Collections.Generic;
using System.Linq;
using WEB_SH.Models;
using WEB_SH.Repository;

namespace WEB_SH.Services
{
    public class ProfilService
    {
        private readonly PersonneRepos _personneRepos;
        public ProfilService(PersonneRepos personneRepos)
        {
            _personneRepos = personneRepos;
        }

        public List<ProfilIndexViewModel> GetProfilsIndex()
        {
            var profils = _personneRepos.GetAll() ?? new List<WEB_SH.Entities.Personne>();

            var profilsIndex = profils.Select(p => new ProfilIndexViewModel
            {
                Id = p.Id,
                NomComplet = $"{p.Prenom} {p.Nom}".Trim(),
                Bio = p.Bio ?? string.Empty
            }).ToList();

            return profilsIndex;
        }
    }
}
