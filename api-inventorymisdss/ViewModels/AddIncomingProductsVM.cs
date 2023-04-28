using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using Microsoft.EntityFrameworkCore;

namespace api_inventorymisdss.ViewModels
{
    public class AddIncomingProductsVM
    {
        public DateTime DateTimeRestock { get; set; }
        public int StockCount { get; set; }
    }
}