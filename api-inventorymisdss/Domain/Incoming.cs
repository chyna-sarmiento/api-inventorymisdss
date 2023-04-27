namespace api_inventorymisdss.Domain
{
    public class Incoming
    {
        public int Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public int StockCount { get; set; }

        public Product? Product { get; set; }
        public long ProductId { get; set; }
    }
}