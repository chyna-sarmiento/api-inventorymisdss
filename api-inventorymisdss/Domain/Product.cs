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
        public DateTime LastUpdated { get; set; }

        private Price _price = new Price();
        public Price Price
        {
            get { return _price; }
            set
            {
                _price = value ?? new Price { SellingPrice = value.SellingPrice };
            }
        }
    }

    public class Price
    {
        public decimal SellingPrice { get; set; } //required
        public decimal? WholesalePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? MarkupPercent { get; set; }

        public Product? Product { get; set; }
        public long ProductId { get; set; }
    }
}