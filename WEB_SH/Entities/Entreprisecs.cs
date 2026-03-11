using System;

namespace WEB_SH.Entities
{
    public class Entreprise : Base
    {
        public string Nom { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public int CreePar { get; set; }
        public bool? EstSupprime { get; set; }
        public DateTime? SupprimeA { get; set; }
        public int? SupprimePar { get; set; }
    }
}