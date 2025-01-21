using System.ComponentModel.DataAnnotations.Schema;

namespace URL_Shortener.DAL.Entities
{
    public class Url
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long UrlId { get; set; } = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);

        public string UrlText { get; set; }
        public string ShortenUrl { get; set; }
        public string UrlCreationDate { get; set; }
        public string? UrlDescription { get; set; }

        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
