namespace api_inventorymisdss.ViewModels
{
    public class AddNewProductVM
    {
        public string? BarcodeId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string VariantName { get; set; }
        public string? Measurement { get; set; } //optional
        public decimal Price { get; set; }
    }
}