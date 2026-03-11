using Microsoft.EntityFrameworkCore;
using WEB_SH.Data;
using WEB_SH.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WEB_SH.Repository
{
    public class MessageRepos
    {
        private readonly MyDbContextPortfolio _context;
        private readonly ILogger<MessageRepos> _logger;

        public MessageRepos(MyDbContextPortfolio context, ILogger<MessageRepos> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Message> GetAll()
        {
            return _context.Messages
                .Include(m => m.Expediteur)
                .Include(m => m.Destinataire)
                .ToList();
        }

        public Message GetById(int id)
        {
            return _context.Messages
                .Include(m => m.Expediteur)
                .Include(m => m.Destinataire)
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Message> GetMessagesPourUtilisateur(int userId)
        {
            return _context.Messages
                .Include(m => m.Expediteur)
                .Include(m => m.Destinataire)
                .Where(m => m.DestinataireId == userId)
                .OrderByDescending(m => m.DateEnvoi)
                .ToList();
        }

        public void Add(Message message)
        {
            try
            {
                _logger.LogInformation($"Ajout du message - Expéditeur: {message.ExpediteurId}, Destinataire: {message.DestinataireId}");

                _context.Messages.Add(message);

                _logger.LogInformation("Sauvegarde dans la base de données...");

                _context.SaveChanges();

                _logger.LogInformation($"Message sauvegardé avec ID: {message.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERREUR lors de l'ajout du message: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public void Delete(int id)
        {
            var message = _context.Messages.Find(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        }
    }
}
