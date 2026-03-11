using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_SH.Data;
using WEB_SH.Entities;
using WEB_SH.Repository;
using WEB_SH.Shered;
using System;
using System.IO;
using System.Linq;

namespace WEB_SH.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly MyDbContextPortfolio _context;
        private readonly PersonneRepos _personneRepos;
        private readonly CompetenceRepos _competenceRepos;
        private readonly ProjetRepos _projetRepos;

        public PortfolioController(
            MyDbContextPortfolio context,
            PersonneRepos personneRepos,
            CompetenceRepos competenceRepos,
            ProjetRepos projetRepos)
        {
            _context = context;
            _personneRepos = personneRepos;
            _competenceRepos = competenceRepos;
            _projetRepos = projetRepos;
        }

        // PAGE 5: Liste des portfolios pour recruteur
        public IActionResult ListeRecruteur()
        {
            var candidats = _context.Personnes
                .Where(p => p.Role == "Candidat")
                .Include(p => p.Competences)
                .Include(p => p.Projets)
                .ToList();
            return View(candidats);
        }

        // PAGE 6: Dashboard du candidat
        public IActionResult DashboardCandidat()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var personne = _context.Personnes
                .Include(p => p.Competences)
                .Include(p => p.Projets)
                .FirstOrDefault(p => p.Id == userId.Value);

            return View(personne);
        }

        // PAGE 7: Détails d'un portfolio
        public IActionResult Details(int id)
        {
            var personne = _context.Personnes
                .Include(p => p.Competences)
                .Include(p => p.Projets)
                .FirstOrDefault(p => p.Id == id);

            if (personne == null)
                return NotFound();

            return View(personne);
        }

        // ACTION UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Personne personne, IFormFile photoFile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var existingPersonne = _context.Personnes.Find(userId.Value);
            if (existingPersonne == null)
                return NotFound();

            // Mise à jour des propriétés
            existingPersonne.Nom = personne.Nom;
            existingPersonne.Prenom = personne.Prenom;
            existingPersonne.Email = personne.Email;
            existingPersonne.Telephone = personne.Telephone;
            existingPersonne.Bio = personne.Bio;

            // Gestion de la photo
            if (photoFile != null && photoFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{photoFile.FileName}";
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photoFile.CopyTo(stream);
                }

                existingPersonne.Photo = $"/uploads/{fileName}";
            }

            _context.SaveChanges();
            TempData["Success"] = "Profil mis à jour avec succès!";
            return RedirectToAction("DashboardCandidat");
        }

        // Ajouter une compétence 
        [HttpPost]
        public IActionResult AjouterCompetence(string nom, string niveau)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Non connecté" });

            if (string.IsNullOrEmpty(nom))
                return Json(new { success = false, message = "Le nom est requis" });

            if (string.IsNullOrEmpty(niveau))
                return Json(new { success = false, message = "Le niveau est requis" });

            // Mappage des niveaux textuels vers l'enum
            NiveauCompetence niveauEnum;
            switch (niveau.ToLower())
            {
                case "debutant":
                    niveauEnum = NiveauCompetence.Debutant;
                    break;
                case "intermediaire":
                    niveauEnum = NiveauCompetence.Intermediaire;
                    break;
                case "avance":
                    niveauEnum = NiveauCompetence.Avance;
                    break;
                case "expert":
                    niveauEnum = NiveauCompetence.Expert;
                    break;
                default:
                    return Json(new { success = false, message = "Niveau invalide. Choisissez parmi: Debutant, Intermediaire, Avance, Expert" });
            }

            var competence = new Competence
            {
                Nom = nom,
                Niveau = niveauEnum,
                PersonneId = userId.Value
            };

            _competenceRepos.Create(competence);
            return Json(new { success = true });
        }

        // Supprimer une compétence 
        [HttpPost]
        public IActionResult SupprimerCompetence(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Non connecté" });

            var competence = _competenceRepos.GetById(id);
            if (competence == null)
                return Json(new { success = false, message = "Compétence non trouvée" });

            if (competence.PersonneId != userId.Value)
                return Json(new { success = false, message = "Non autorisé" });

            _competenceRepos.Delete(id);
            return Json(new { success = true });
        }

        // Ajouter un projet 
        [HttpPost]
        public IActionResult AjouterProjet(string titre, string description, string lien)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Non connecté" });

            if (string.IsNullOrEmpty(titre))
                return Json(new { success = false, message = "Le titre est requis" });

            if (string.IsNullOrEmpty(description))
                return Json(new { success = false, message = "La description est requise" });

            var projet = new Projet
            {
                Titre = titre,
                Description = description,
                Lien = lien ?? "",
                PersonneId = userId.Value
            };

            _projetRepos.Add(projet);
            return Json(new { success = true });
        }

        // Supprimer un projet 
        [HttpPost]
        public IActionResult SupprimerProjet(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Non connecté" });

            var projet = _projetRepos.GetById(id);
            if (projet == null)
                return Json(new { success = false, message = "Projet non trouvé" });

            if (projet.PersonneId != userId.Value)
                return Json(new { success = false, message = "Non autorisé" });

            _projetRepos.Delete(id);
            return Json(new { success = true });
        }
    }
}