// Fonctions pour la gestion du portfolio

function ajouterCompetence() {
    var nom = prompt("Entrez le nom de la compétence:");
    if (nom) {
        $.ajax({
            url: '/Portfolio/AjouterCompetence',
            type: 'POST',
            data: { nom: nom },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert("Erreur lors de l'ajout");
                }
            }
        });
    }
}

function supprimerCompetence(id) {
    if (confirm("Êtes-vous sûr de vouloir supprimer cette compétence?")) {
        $.ajax({
            url: '/Portfolio/SupprimerCompetence',
            type: 'POST',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert("Erreur lors de la suppression");
                }
            }
        });
    }
}

function ajouterProjet() {
    var titre = prompt("Titre du projet:");
    if (titre) {
        var description = prompt("Description:");
        $.ajax({
            url: '/Portfolio/AjouterProjet',
            type: 'POST',
            data: { titre: titre, description: description },
            success: function (response) {
                if (response.success) {
                    location.reload();
                }
            }
        });
    }
}

function supprimerProjet(id) {
    if (confirm("Êtes-vous sûr de vouloir supprimer ce projet?")) {
        $.ajax({
            url: '/Portfolio/SupprimerProjet',
            type: 'POST',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    location.reload();
                }
            }
        });
    }
}