namespace api_inventorymisdss.ViewModels
{
    public class OutgoingListVM
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeOutgoing { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}