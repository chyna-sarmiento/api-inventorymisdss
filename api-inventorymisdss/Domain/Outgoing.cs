namespace api_inventorymisdss.Domain
{
    public class Outgoing
    {
        public long Id { get; set; }
        public DateTime DateTimeOutgoing { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime LastUpdated { get; set; }

        public Product? Product { get; set; }
        public long ProductId { get; set; }
    }
}