using api_inventorymisdss.ViewModels;

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
        public decimal Price { get; set; }
        public int StockCount { get; set; } //optional
        public DateTime LastUpdated { get; set; }

        public Product(string barcodeId, string brand, string name, string variantName, string measurement, decimal price, int stockCount)
        {
            BarcodeId = barcodeId;
            Brand = brand;
            Name = name;
            VariantName = variantName;
            Measurement = measurement;
            Price = price;
            StockCount = stockCount;
            LastUpdated = DateTime.Now;
        }
    }
}