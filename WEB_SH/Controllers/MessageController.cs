using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using WEB_SH.Entities;
using WEB_SH.Repository;
using System;

namespace WEB_SH.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageRepos _messageRepos;
        private readonly PersonneRepos _personneRepos;
        private readonly ILogger<MessageController> _logger; 

        public MessageController(
            MessageRepos messageRepos,
            PersonneRepos personneRepos,
            ILogger<MessageController> logger) 
        {
            _messageRepos = messageRepos;
            _personneRepos = personneRepos;
            _logger = logger; 
        }

        // PAGE 8: Messagerie
        public IActionResult Index(int? destinataireId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // LOG
            _logger.LogInformation($"Index appelé - UserId: {userId}, DestinataireId: {destinataireId}");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (destinataireId.HasValue)
            {
                // LOG
                _logger.LogInformation($"Recherche du destinataire ID: {destinataireId.Value}");

                var destinataire = _personneRepos.Get(destinataireId.Value); 

                // LOG
                _logger.LogInformation($"Destinataire trouvé: {destinataire != null}");

                if (destinataire != null)
                {
                    ViewBag.DestinataireId = destinataireId;
                    ViewBag.DestinataireName = $"{destinataire.Prenom} {destinataire.Nom}";
                }
            }

            var messages = _messageRepos.GetMessagesPourUtilisateur(userId.Value);

            // LOG
            _logger.LogInformation($"Nombre de messages chargés: {messages.Count}");

            return View(messages);
        }

        // Envoyer un message
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Envoyer(int destinataireId, string objet, string contenu)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                // LOG
                _logger.LogInformation($"Envoyer appelé - UserId: {userId}, DestinataireId: {destinataireId}");

                if (userId == null)
                {
                    _logger.LogWarning("Tentative d'envoi sans authentification");
                    return Json(new { success = false, message = "Non authentifié" });
                }

                // LOG des données
                _logger.LogInformation($"Objet: {objet}");
                _logger.LogInformation($"Contenu: {contenu}");

                
                if (string.IsNullOrWhiteSpace(objet))
                {
                    _logger.LogWarning("Objet vide");
                    return Json(new { success = false, message = "L'objet est requis" });
                }

                if (string.IsNullOrWhiteSpace(contenu))
                {
                    _logger.LogWarning("Contenu vide");
                    return Json(new { success = false, message = "Le contenu est requis" });
                }

                var message = new Message
                {
                    ExpediteurId = userId.Value,
                    DestinataireId = destinataireId,
                    Objet = objet,
                    Contenu = contenu,
                    DateEnvoi = DateTime.Now
                };

                _logger.LogInformation("Tentative d'ajout du message...");

                _messageRepos.Add(message);

                _logger.LogInformation($"Message ajouté avec succès! ID: {message.Id}");

                return Json(new { success = true, message = "Message envoyé avec succès!" });
            }
            catch (Exception ex)
            {
                // LOG DE L'ERREUR COMPLÈTE
                _logger.LogError($"ERREUR dans Envoyer: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
                _logger.LogError($"Type d'erreur: {ex.GetType().Name}");

                return Json(new
                {
                    success = false,
                    message = $"Erreur: {ex.Message}"
                });
            }
        }
    }
}