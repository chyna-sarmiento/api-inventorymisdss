using api_inventorymisdss.ViewModels;

namespace api_inventorymisdss.Domain
{
    public class ProductList
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }

        public Product Product { get; set; }
        public long ProductId { get; set; }

        public ProductList(Product product)
        {
            DisplayName = $"{product.Brand} {product.Name} {product.VariantName} ({product.Measurement})";
        }
    }
}