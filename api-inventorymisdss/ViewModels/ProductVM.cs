using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class ProductVM
    {
        [MaxLength(43)]
        public string? BarcodeId { get; set; }

        [MaxLength(160)]
        public string Brand { get; set; }

        [Required, MaxLength(160)]
        public string Name { get; set; }

        [Required, MaxLength(160)]
        public string VariantName { get; set; }

        [MaxLength(30)]
        public string? Measurement { get; set; }

        [Required, MaxLength(30)]
        public decimal Price { get; set; }
    }
}