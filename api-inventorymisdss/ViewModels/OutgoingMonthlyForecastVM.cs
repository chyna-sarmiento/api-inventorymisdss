using api_inventorymisdss.Domain;
using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingMonthlyForecastVM
    {
        public int OutgoingYear { get; internal set; }
        public int OutgoingMonth { get; internal set; }

        [Required]
        public long OutgoingProductId { get; set; }
        public string ProductName { get; internal set; }
        public int OutgoingDemandVolume { get; set; }
    }
}