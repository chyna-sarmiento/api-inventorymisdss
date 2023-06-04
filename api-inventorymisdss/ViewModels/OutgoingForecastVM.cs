using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingForecastVM
    {
        [Required]
        public long OutgoingProductId { get; set; }
        public string ProductName { get; set; }
        public int OutgoingDemandVolume { get; set; }
    }
}