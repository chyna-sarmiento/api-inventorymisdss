using api_inventorymisdss.Domain;
using System.ComponentModel.DataAnnotations;

namespace api_inventorymisdss.ViewModels
{
    public class OutgoingWeeklyForecastVM
    {
        public int OutgoingYear { get; internal set; }
        public int OutgoingWeek { get; internal set; }

        [Required]
        public long OutgoingProductId { get; set; }
        public string ProductName { get; internal set; }
        public int OutgoingDemandVolume { get; set; }
    }
}