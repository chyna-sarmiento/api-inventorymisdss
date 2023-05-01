using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Domain
{
    public class Incoming
    {
        public long Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public int IncomingStockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

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