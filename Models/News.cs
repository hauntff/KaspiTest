namespace KaspiTest.Models
{
    public class News
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; }
        public DateTime? Time { get; set; }
        public string Link { get; set; }
        public string Details { get; set; }
    }
}
