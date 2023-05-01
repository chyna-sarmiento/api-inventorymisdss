namespace api_inventorymisdss.ViewModels
{
    public class IncomingListVM
    {
        public long Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public string ProductName { get; set; }
        public int IncomingStockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}