using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;

namespace api_inventorymisdss.Controllers;

public static class IncomingController
{
    public static void MapIncomingEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Incoming").WithTags(nameof(Incoming));

        group.MapPost("/", async (IncomingProductVM appData, ApplicationContext db) =>
        {
            var product = await db.Products.FindAsync(appData.IncomingProductId);
            var IncomingProduct = new Incoming(appData.IncomingProductId, appData.IncomingStockQuantity);

            product.StockCount += appData.IncomingStockQuantity;
            product.LastUpdated = DateTime.UtcNow;

            db.Incomings.Add(IncomingProduct);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Incoming/{IncomingProduct.Id}", IncomingProduct);
        })
        .WithName("CreateIncoming")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, IncomingProductVM appData, ApplicationContext db) =>
        {
            var preIncoming = await db.Incomings.FindAsync(id);
            var product = await db.Products.FindAsync(appData.IncomingProductId);

            var affected = await db.Incomings
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.IncomingProductId, appData.IncomingProductId)
                  .SetProperty(m => m.IncomingStockQuantity, appData.IncomingStockQuantity)
                  .SetProperty(m => m.LastUpdated, DateTime.UtcNow)
                );

            int diffQuantity = preIncoming.IncomingStockQuantity - appData.IncomingStockQuantity;

            if(diffQuantity < 0)
            {
                product.StockCount -= diffQuantity;
            }
            else
            {
                product.StockCount += diffQuantity;
            }
            product.LastUpdated = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateIncoming")
        .WithOpenApi();

        group.MapGet("/List", async (ApplicationContext db) =>
        {
            var incomingList = await db.Incomings.Select(i => new IncomingListVM
            {
                Id = i.Id,
                DateTimeRestock = i.DateTimeRestock,
                ProductName = $"{i.Product.Brand} {i.Product.Name} {i.Product.VariantName} ({i.Product.Measurement})",
                IncomingStockQuantity = i.IncomingStockQuantity,
                LastUpdated = i.LastUpdated
            }).ToListAsync();

            return incomingList;
        })
        .WithName("GetIncomingList")
        .WithOpenApi();

        group.MapGet("/", async (ApplicationContext db) =>
        {
            return await db.Incomings.ToListAsync();
        })
        .WithName("GetAllIncomings")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Incoming>, NotFound>> (long id, ApplicationContext db) =>
        {
            return await db.Incomings.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Incoming model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetIncomingById")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (long id, ApplicationContext db) =>
        {
            var affected = await db.Incomings
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteIncoming")
        .WithOpenApi();
    }
}
