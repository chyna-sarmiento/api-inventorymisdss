namespace api_inventorymisdss.Domain
{
    public class ProductList
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }

        public static ProductList FromProduct(Product product)
        {
            return new ProductList
            {
                Id = product.Id,
                DisplayName = $"{product.Brand} {product.Name} {product.VariantName} ({product.Measurement})"
            };
        }
    }
}