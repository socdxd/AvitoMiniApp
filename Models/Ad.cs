namespace AvitoMiniApp.Models
{
    public class Ad
    {
        public int AdId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
    }
}
