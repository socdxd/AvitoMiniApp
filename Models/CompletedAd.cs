namespace AvitoMiniApp.Models
{
    public class CompletedAd
    {
        public int CompletedAdId { get; set; }
        public int AdId { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Title { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Profit { get; set; }
    }
}
