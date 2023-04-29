using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Domain
{
    public class Incoming
    {
        public long Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public List<ProductList> ProductName { get; set; }
        public int IncomingStockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

        public ProductList ProductList { get; set; }
        public int IncomingProductId { get; set; }

        public Incoming()
        {
            ProductName = new List<ProductList>();
        }

        public Incoming(List<ProductList> productName, int incomingStockQuantity)
        {
            ProductName = productName;
            IncomingStockQuantity = incomingStockQuantity;
            DateTimeRestock = DateTime.Now;
        }
    }
}