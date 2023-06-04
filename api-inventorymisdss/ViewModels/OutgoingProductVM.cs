using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingProductVM
    {
        [Required]
        public long OutgoingProductId { get; set; }

        [Required, MaxLength(7)]
        public int Quantity { get; set; }
        
        public DateTime DateTimeOutgoing { get; set; }
    }
}