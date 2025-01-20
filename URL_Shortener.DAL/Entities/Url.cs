namespace URL_Shortener.DAL.Entities
{
    public class Url
    {
        public Guid UrlId { get; set; } = Guid.NewGuid();
        public string UrlText { get; set; }
        public string ShortenUrl { get; set; }
        public DateTime UrlCreationDate { get; set; }
        public string? UrlDescription { get; set; }

        public UserAccount UserAccount { get; set; }
    }
}
