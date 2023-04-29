using api_inventorymisdss.Domain;
using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingProductVM
    {
        [Required]
        public List<ProductList> ProductName { get; set; }

        [Required, MaxLength(7)]
        public int Quantity { get; set; }
    }
}