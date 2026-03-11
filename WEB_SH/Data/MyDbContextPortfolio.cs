using Microsoft.EntityFrameworkCore;
using WEB_SH.Entities;

namespace WEB_SH.Data
{
    public class MyDbContextPortfolio : DbContext
    {
        public MyDbContextPortfolio(DbContextOptions<MyDbContextPortfolio> options) : base(options)
        {
        }

        public DbSet<Projet> Projet { get; set; }
        public DbSet<Competence> Competence { get; set; }
        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration pour Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Expediteur)
                .WithMany(p => p.MessagesEnvoyes)
                .HasForeignKey(m => m.ExpediteurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Destinataire)
                .WithMany(p => p.MessagesRecus)
                .HasForeignKey(m => m.DestinataireId)
                .OnDelete(DeleteBehavior.Restrict);

            

            // Configuration pour Competence
            modelBuilder.Entity<Competence>()
                .HasOne(c => c.Personne)
                .WithMany(p => p.Competences)
                .HasForeignKey(c => c.PersonneId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configuration pour Projet
            modelBuilder.Entity<Projet>()
                .HasOne(p => p.Personne)
                .WithMany(p => p.Projets)
                .HasForeignKey(p => p.PersonneId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Mappage de colonne pour Telephone
            modelBuilder.Entity<Personne>()
                .Property(p => p.Telephone)
                .HasColumnName("NumeroTel");
        }
    }
}