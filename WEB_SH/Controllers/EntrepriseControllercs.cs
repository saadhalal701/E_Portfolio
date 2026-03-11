using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WEB_SH.Entities;

namespace WEB_SH.Controllers
{
    public class EntrepriseController : Controller
    {
        public IActionResult ListeEntreprise()
        {
            var entreprises = new List<Entreprise>
            {
                new Entreprise { Id = 1, Nom = "TechCorp", DateCreation = new DateTime(2010,5,1), CreePar = 1, EstSupprime = false },
                new Entreprise { Id = 2, Nom = "Innova Solutions", DateCreation = new DateTime(2015,8,12), CreePar = 1, EstSupprime = false },
                new Entreprise { Id = 3, Nom = "GreenWorks SA", DateCreation = new DateTime(2008,3,20), CreePar = 2, EstSupprime = false },
                new Entreprise { Id = 4, Nom = "BlueOcean Ltd", DateCreation = new DateTime(2020,1,10), CreePar = 2, EstSupprime = false },
                new Entreprise { Id = 5, Nom = "Alpha & Co", DateCreation = new DateTime(2000,11,5), CreePar = 3, EstSupprime = false }
            };

            return View(entreprises);
        }
    }
}