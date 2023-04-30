using Microsoft.EntityFrameworkCore;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
namespace api_inventorymisdss.Controllers;

public static class ProductListController
{
    public static void MapProductListEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ProductList").WithTags(nameof(ProductList));

        group.MapGet("/", async (ApplicationContext db) =>
        {
            return await db.ProductList.ToListAsync();
        })
        .WithName("GetAllProductLists")
        .WithOpenApi();
    }
}