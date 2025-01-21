namespace URL_Shortener.DAL.Entities
{
    public class Url
    {
        public long UrlId { get; set; }
        public string UrlText { get; set; }
        public string ShortenUrl { get; set; }
        public string UrlCreationDate { get; set; }
        public string? UrlDescription { get; set; }

        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
