using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;

namespace api_inventorymisdss.Controllers;

public static class ProductsController
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

        group.MapPost("/", async (ProductVM appData, ApplicationContext db) =>
        {
            var NewProduct = new Product(
                appData.BarcodeId,
                appData.Brand,
                appData.Name,
                appData.VariantName,
                appData.Measurement,
                appData.Price,
                appData.StockCount
            );

            db.Products.Add(NewProduct);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/Product/{NewProduct.Id}", NewProduct);
        })
        .WithName("CreateProduct")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, ProductVM appData, ApplicationContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.BarcodeId, appData.BarcodeId)
                  .SetProperty(m => m.Brand, appData.Brand)
                  .SetProperty(m => m.Name, appData.Name)
                  .SetProperty(m => m.VariantName, appData.VariantName)
                  .SetProperty(m => m.Measurement, appData.Measurement)
                  .SetProperty(m => m.StockCount, appData.StockCount)
                  .SetProperty(m => m.Price, appData.Price)
                  .SetProperty(m => m.LastUpdated, DateTime.Now)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        group.MapGet("/", async (ApplicationContext db) =>
        {
            var productList = await db.Products.Select(p => new ProductListVM
            {
                Id = p.Id,
                DisplayName = $"{p.Brand} {p.Name} {p.VariantName} ({p.Measurement})"
            }).ToListAsync();

            return productList;
        })
        .WithName("GetProductList")
        .WithOpenApi();

        group.MapGet("/", async (ApplicationContext db) =>
            {
                return await db.Products.ToListAsync();
            })
            .WithName("GetAllProducts")
            .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (long id, ApplicationContext db) =>
        {
            return await db.Products.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Product model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProductById")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (long id, ApplicationContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProduct")
        .WithOpenApi();
    }
}