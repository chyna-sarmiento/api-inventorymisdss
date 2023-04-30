using api_inventorymisdss.ViewModels;
using Microsoft.Identity.Client;

namespace api_inventorymisdss.Domain
{
    public class Outgoing
    {
        public long Id { get; set; }
        public List<ProductList> ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeOutgoing { get; set; }
        public DateTime LastUpdated { get; set; }

        public ProductList ProductList { get; set; }
        public int OutgoingProductId { get; set; }

        public Outgoing()
        {
            ProductName = new List<ProductList>();
        }

        public Outgoing(List<ProductList> productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
            DateTimeOutgoing = DateTime.Now;
        }   
    }
}