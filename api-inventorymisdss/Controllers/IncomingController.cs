using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
namespace api_inventorymisdss.Controllers;

public static class IncomingController
{
    public static void MapIncomingEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Incoming").WithTags(nameof(Incoming));

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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, Incoming incoming, ApplicationContext db) =>
        {
            var affected = await db.Incomings
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, incoming.Id)
                  .SetProperty(m => m.DateTimeRestock, incoming.DateTimeRestock)
                  .SetProperty(m => m.IncomingStockQuantity, incoming.IncomingStockQuantity)
                  .SetProperty(m => m.LastUpdated, incoming.LastUpdated)
                  .SetProperty(m => m.IncomingProductId, incoming.IncomingProductId)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateIncoming")
        .WithOpenApi();

        group.MapPost("/", async (Incoming incoming, ApplicationContext db) =>
        {
            db.Incomings.Add(incoming);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Incoming/{incoming.Id}",incoming);
        })
        .WithName("CreateIncoming")
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
