using System.ComponentModel.DataAnnotations.Schema;

namespace api_inventorymisdss.Domain
{
    public class Incoming
    {
        public long Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public int IncomingStockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("IncomingProductId")]
        public Product Product { get; set; }
        public long IncomingProductId { get; set; }

        public Incoming(long incomingProductId, int incomingStockQuantity)
        {
            IncomingProductId = incomingProductId;
            IncomingStockQuantity = incomingStockQuantity;
            DateTimeRestock = DateTime.UtcNow;
        }
    }
}