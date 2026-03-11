using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WEB_SH.Entities;
using WEB_SH.Repository;
using System.Linq;
using System;

namespace WEB_SH.Controllers
{
    public class AccountController : Controller
    {
        private readonly PersonneRepos _personneRepos;
        private readonly IPasswordHasher<Personne> _passwordHasher;

        public AccountController(PersonneRepos personneRepos, IPasswordHasher<Personne> passwordHasher)
        {
            _personneRepos = personneRepos;
            _passwordHasher = passwordHasher;
        }

        // PAGE 2: Choix du rôle
        public IActionResult ChoixRole()
        {
            return View();
        }

        // PAGE 3: Inscription (GET)
        public IActionResult Register(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                TempData["Error"] = "Veuillez sélectionner un rôle";
                return RedirectToAction("ChoixRole");
            }

            var model = new Personne
            {
                Role = role
            };

            ViewBag.Role = role;
            return View(model);
        }

        // PAGE 3: Inscription (POST) 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Personne personne)
        {
            // Débogage : ne JAMAIS afficher le mot de passe en clair en production
            Console.WriteLine($"=== TENTATIVE INSCRIPTION ===");
            Console.WriteLine($"Nom: {personne.Nom}");
            Console.WriteLine($"Prénom: {personne.Prenom}");
            Console.WriteLine($"Email: {personne.Email}");
            Console.WriteLine($"Téléphone: {personne.Telephone}");
            Console.WriteLine($"Rôle: {personne.Role}");
            Console.WriteLine($"ModelState valid: {ModelState.IsValid}");
            Console.WriteLine($"====================");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }

                ViewBag.Role = personne.Role;
                return View(personne);
            }

            try
            {
                
                var plain = personne.MotDePasse ?? string.Empty;
                personne.MotDePasse = _passwordHasher.HashPassword(personne, plain);

                bool created = _personneRepos.Create(personne);

                if (!created)
                {
                    ModelState.AddModelError("", "Erreur lors de la création du compte.");
                    ViewBag.Role = personne.Role;
                    return View(personne);
                }

                Console.WriteLine($"Personne créée avec ID: {personne.Id}");

                HttpContext.Session.SetInt32("UserId", personne.Id);
                HttpContext.Session.SetString("UserRole", personne.Role);
                HttpContext.Session.SetString("UserName", $"{personne.Prenom} {personne.Nom}");

                TempData["Success"] = "Compte créé avec succès !";

                if (personne.Role == "Candidat")
                {
                    return RedirectToAction("DashboardCandidat", "Portfolio");
                }
                else
                {
                    return RedirectToAction("ListeRecruteur", "Portfolio");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERREUR dans Register: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                ModelState.AddModelError("", $"Une erreur est survenue: {ex.Message}");
                ViewBag.Role = personne.Role;
                return View(personne);
            }
        }

        // PAGE 4: Connexion (GET)
        public IActionResult Login()
        {
            return View();
        }

        // PAGE 4: Connexion (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string motDePasse, string role)
        {
            // Chercher l'utilisateur par email + role (requête côté BD)
            var personne = _personneRepos.GetByEmailAndRole(email, role);

            if (personne != null)
            {
                PasswordVerificationResult result;

                try
                {
                    result = _passwordHasher.VerifyHashedPassword(personne, personne.MotDePasse, motDePasse);
                }
                catch (FormatException)
                {
                    
                    if (!string.IsNullOrEmpty(personne.MotDePasse) && personne.MotDePasse == motDePasse)
                    {
                        personne.MotDePasse = _passwordHasher.HashPassword(personne, motDePasse);
                        _personneRepos.Update(personne);
                        result = PasswordVerificationResult.Success;
                    }
                    else
                    {
                        result = PasswordVerificationResult.Failed;
                    }
                }

                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        personne.MotDePasse = _passwordHasher.HashPassword(personne, motDePasse);
                        _personneRepos.Update(personne);
                    }

                    HttpContext.Session.SetInt32("UserId", personne.Id);
                    HttpContext.Session.SetString("UserRole", personne.Role);
                    HttpContext.Session.SetString("UserName", $"{personne.Prenom} {personne.Nom}");

                    if (role == "Candidat")
                    {
                        return RedirectToAction("DashboardCandidat", "Portfolio");
                    }
                    else
                    {
                        return RedirectToAction("ListeRecruteur", "Portfolio");
                    }
                }
            }

            ViewBag.Error = "Email, mot de passe ou rôle incorrect";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "Vous avez été déconnecté avec succès";
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Entreprise()
        {
            return RedirectToAction("ListeEntreprise", "Entreprise");
        }
    }
}