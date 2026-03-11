using System.Collections.Generic;
using System.Linq;
using WEB_SH.Data;
using WEB_SH.Entities;
using WEB_SH.Shered;

namespace WEB_SH.Repository
{
    public class CompetenceRepos
    {
        private readonly MyDbContextPortfolio _db;

        public CompetenceRepos(MyDbContextPortfolio context)
        {
            _db = context;
        }

        // Create avec validation
        public Competence Create(Competence obj)
        {
            // Validation pour éviter les NULL
            if (string.IsNullOrWhiteSpace(obj.Nom))
            {
                throw new ArgumentException("Le titre de la compétence est requis et ne peut pas être vide.");
            }

            // Validation optionnelle pour d'autres champs requis
            if (string.IsNullOrWhiteSpace(obj.Nom))
            {
                throw new ArgumentException("Le nom de la compétence est requis.");
            }

            _db.Competence.Add(obj);
            _db.SaveChanges();
            return obj;
        }

        // Update avec validation
        public Competence Update(Competence obj)
        {
            // Vérifier si la compétence existe
            var existingCompetence = _db.Competence.Find(obj.Id);
            if (existingCompetence == null)
            {
                throw new KeyNotFoundException($"Compétence avec ID {obj.Id} non trouvée.");
            }

            // Validation
            if (string.IsNullOrWhiteSpace(obj.Nom))
            {
                throw new ArgumentException("Le titre de la compétence est requis et ne peut pas être vide.");
            }

            if (string.IsNullOrWhiteSpace(obj.Nom))
            {
                throw new ArgumentException("Le nom de la compétence est requis.");
            }

            // Mise à jour
            _db.Entry(existingCompetence).CurrentValues.SetValues(obj);
            _db.SaveChanges();
            return obj;
        }

        // Delete
        public void Delete(int id)
        {
            var competence = _db.Competence.Find(id);
            if (competence != null)
            {
                _db.Competence.Remove(competence);
                _db.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Compétence avec ID {id} non trouvée.");
            }
        }

        // Read by Id
        public Competence? GetById(int id)
        {
            return _db.Competence.Find(id);
        }

        // Read All
        public List<Competence> GetAll()
        {
            return _db.Competence.ToList();
        }

        public List<Competence> GetAllByNiveau(NiveauCompetence niveau)
        {
            return _db.Competence
                .Where(c => c.Niveau == niveau)
                .OrderBy(c => c.Nom)
                .ToList();
        }
    }
}