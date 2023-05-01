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
            var IncomingProduct = new Incoming(appData.IncomingProductId, appData.IncomingStockQuantity);

            db.Incomings.Add(IncomingProduct);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Incoming/{IncomingProduct.Id}", IncomingProduct);
        })
        .WithName("CreateIncoming")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, IncomingProductVM appData, ApplicationContext db) =>
        {
            var affected = await db.Incomings
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.IncomingProductId, appData.IncomingProductId)
                  .SetProperty(m => m.IncomingStockQuantity, appData.IncomingStockQuantity)
                  .SetProperty(m => m.LastUpdated, DateTime.Now)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateIncoming")
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
