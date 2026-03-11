using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WEB_SH.Data;
using WEB_SH.Entities;

namespace WEB_SH.Repository
{
    public class PersonneRepos
    {
        private readonly MyDbContextPortfolio _db;

        public PersonneRepos(MyDbContextPortfolio context)
        {
            _db = context;
        }

        public bool Create(Personne personne)
        {
            if (personne == null) return false;

            try
            {
                _db.Personnes.Add(personne);
                int result = _db.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur création personne: {ex.Message}");
                return false;
            }
        }

        public bool Update(Personne personne)
        {
            if (personne == null || personne.Id <= 0) return false;

            _db.Personnes.Update(personne);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var p = _db.Personnes.Find(id);
            if (p == null) return false;

            _db.Personnes.Remove(p);
            _db.SaveChanges();
            return true;
        }

        public Personne? Get(int id)
        {
            return _db.Personnes.Find(id);
        }

        public List<Personne> GetAll()
        {
            return _db.Personnes.ToList();
        }

        public Personne? GetById(int id)
        {
            return _db.Personnes
                .Include(p => p.Competences)
                .Include(p => p.Projets)
                .FirstOrDefault(p => p.Id == id);
        }

        public Personne? GetByEmailAndRole(string email, string role)
        {
            return _db.Personnes.FirstOrDefault(p => p.Email == email && p.Role == role);
        }
    }
}
