using api_inventorymisdss.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;

namespace api_inventorymisdss.ViewModels
{
    public class AddNewProductVM
    {
        public string? BarcodeId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string VariantName { get; set; }
        public string? Measurement { get; set; } //optional
        public decimal Price { get; set; }
    }


public static class ProductEndpoints
{
	public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, Product product, ApplicationContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  //.SetProperty(m => m.Id, product.Id)
                  .SetProperty(m => m.BarcodeId, product.BarcodeId)
                  .SetProperty(m => m.Brand, product.Brand)
                  .SetProperty(m => m.Name, product.Name)
                  .SetProperty(m => m.VariantName, product.VariantName)
                  .SetProperty(m => m.Measurement, product.Measurement)
                  .SetProperty(m => m.StockCount, product.StockCount)
                  .SetProperty(m => m.Price, product.Price)
                  .SetProperty(m => m.LastUpdated, product.LastUpdated)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        group.MapPost("/", async (Product product, ApplicationContext db) =>
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Product/{product.Id}",product);
        })
        .WithName("CreateProduct")
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
}}