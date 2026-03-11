using WEB_SH.Data;
using WEB_SH.Entities;
using System.Collections.Generic;
using System.Linq;

namespace WEB_SH.Repository
{
    public class ProjetRepos
    {
        private readonly MyDbContextPortfolio _context;

        public ProjetRepos(MyDbContextPortfolio context)
        {
            _context = context;
        }

        public List<Projet> GetAll()
        {
            return _context.Projet.ToList();
        }

        public Projet GetById(int id)
        {
            return _context.Projet.Find(id);
        }

        public void Add(Projet projet)
        {
            _context.Projet.Add(projet);
            _context.SaveChanges();
        }

        public void Update(Projet projet)
        {
            _context.Projet.Update(projet);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var projet = _context.Projet.Find(id);
            if (projet != null)
            {
                _context.Projet.Remove(projet);
                _context.SaveChanges();
            }
        }
    }
}
