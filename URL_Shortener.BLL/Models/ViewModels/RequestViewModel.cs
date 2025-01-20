namespace URL_Shortener.BLL.Models.ViewModels
{
    public class RequestViewModel
    {
        public int Page { get; set; } = 1;
        public string? Text { get; set; }
        public string? SortBy { get; set; }
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
    }
}
