using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class ProductVM
    {
        [MaxLength(43)]
        public string? BarcodeId { get; set; }

        [MaxLength(160)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Provide the name of the product."), MaxLength(160)]
        public string Name { get; set; }

        [MaxLength(160)]
        public string VariantName { get; set; }

        [MaxLength(30)]
        public string? Measurement { get; set; }

        [Required(ErrorMessage = "Provide the price of the product."), Range(1, 1000000, ErrorMessage = "The price must be more than 0.")]
        public decimal Price { get; set; }
    }
}