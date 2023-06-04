using System.ComponentModel.DataAnnotations.Schema;

namespace api_inventorymisdss.Domain
{
    public class Outgoing
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeOutgoing { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("OutgoingProductId")]
        public Product Product { get; set; }
        public long OutgoingProductId { get; set; }

        public Outgoing(long outgoingProductId, int quantity, DateTime dateTimeOutgoing)
        {
            OutgoingProductId = outgoingProductId;
            Quantity = quantity;
            DateTimeOutgoing = dateTimeOutgoing;
            LastUpdated = DateTime.UtcNow;
        }   
    }
}