namespace api_inventorymisdss.Domain
{
    public class Product
    {
        public long Id { get; set; }
        public string? BarcodeId { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public string? VariantName { get; set; }
        public string? Measurement { get; set; } //optional
        public int StockCount { get; set; } //optional
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}