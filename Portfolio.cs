namespace WEB_SH.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Lien { get; set; }
        public DateTime DateCreation { get; set; }
    }
}