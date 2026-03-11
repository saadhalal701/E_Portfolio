using Microsoft.AspNetCore.Mvc;
using WEB_SH.Models;
using WEB_SH.Repository;
using WEB_SH.Services;
using WEB_SH.Entities;

namespace WEB_SH.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ProfilService _profilService;
        private readonly PersonneRepos _personneRepos;

        public ProfilController(PersonneRepos personneRepos, ProfilService profilService)
        {
            _personneRepos = personneRepos;
            _profilService = profilService;
        }

        public IActionResult Index()
        {
            var model = _profilService.GetProfilsIndex();
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new FormulaireCreationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FormulaireCreationViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var personne = new Personne
            {
                Nom = vm.Nom,
                Prenom = vm.Prenom,
                Email = vm.Email ?? string.Empty,  
                Bio = vm.Bio,
                Telephone = vm.NumeroTel,  
                MotDePasse = vm.MotDePasse ?? string.Empty,
                Role = "Candidat" // ou "Recruteur" selon le cas
            };

            var created = _personneRepos.Create(personne);
            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Impossible de créer la personne.");
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}