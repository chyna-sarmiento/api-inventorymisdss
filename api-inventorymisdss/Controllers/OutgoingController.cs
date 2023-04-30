using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
namespace api_inventorymisdss.Controllers;

public static class OutgoingController
{
    public static void MapOutgoingEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Outgoing").WithTags(nameof(Outgoing));

        group.MapGet("/", async (ApplicationContext db) =>
        {
            return await db.Outgoings.ToListAsync();
        })
        .WithName("GetAllOutgoings")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Outgoing>, NotFound>> (long id, ApplicationContext db) =>
        {
            return await db.Outgoings.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Outgoing model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOutgoingById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, Outgoing outgoing, ApplicationContext db) =>
        {
            var affected = await db.Outgoings
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, outgoing.Id)
                  .SetProperty(m => m.Quantity, outgoing.Quantity)
                  .SetProperty(m => m.TotalPrice, outgoing.TotalPrice)
                  .SetProperty(m => m.DateTimeOutgoing, outgoing.DateTimeOutgoing)
                  .SetProperty(m => m.LastUpdated, outgoing.LastUpdated)
                  .SetProperty(m => m.OutgoingProductId, outgoing.OutgoingProductId)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOutgoing")
        .WithOpenApi();

        group.MapPost("/", async (Outgoing outgoing, ApplicationContext db) =>
        {
            db.Outgoings.Add(outgoing);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Outgoing/{outgoing.Id}",outgoing);
        })
        .WithName("CreateOutgoing")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (long id, ApplicationContext db) =>
        {
            var affected = await db.Outgoings
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOutgoing")
        .WithOpenApi();
    }
}
