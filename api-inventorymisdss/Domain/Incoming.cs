using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.Domain
{
    public class Incoming
    {
        public int Id { get; set; }
        public DateTime DateTimeRestock { get; set; }
        public int IncomingStockQuantity { get; set; }
        public int CurrentStockCount { get; set; }

        public Product Product { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }

        public Incoming(int incomingStockQuantity)
        {
            IncomingStockQuantity = incomingStockQuantity;
            CurrentStockCount += IncomingStockQuantity;
            DateTimeRestock = DateTime.Now;
        }
    }
}