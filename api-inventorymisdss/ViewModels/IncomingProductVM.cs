using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class IncomingProductVM
    {
        [Required]
        public List<ProductListVM> ProductName { get; set; }

        [Required, MaxLength(7)]
        public int IncomingStockQuantity { get; set; }
    }
}