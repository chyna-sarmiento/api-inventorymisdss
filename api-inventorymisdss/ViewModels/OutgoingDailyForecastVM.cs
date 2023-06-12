using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingDailyForecastVM
    {
        public DateTime OutgoingDate { get; internal set; }

        [Required]
        public long OutgoingProductId { get; set; }
        public string ProductName { get; internal set; }
        public int OutgoingDemandVolume { get; set; }
    }
}