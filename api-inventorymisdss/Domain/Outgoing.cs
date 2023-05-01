using api_inventorymisdss.ViewModels;
using Microsoft.Identity.Client;

namespace api_inventorymisdss.Domain
{
    public class Outgoing
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeOutgoing { get; set; }
        public DateTime LastUpdated { get; set; }

        public Product Product { get; set; }
        public long OutgoingProductId { get; set; }

        public Outgoing(long outgoingProductId, int quantity)
        {
            OutgoingProductId = outgoingProductId;
            Quantity = quantity;
            DateTimeOutgoing = DateTime.Now;
        }   
    }
}