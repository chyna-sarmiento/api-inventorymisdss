using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;

namespace api_inventorymisdss.Controllers;

public static class OutgoingController
{
    public static void MapOutgoingEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Outgoing").WithTags(nameof(Outgoing));

        //group.MapPost("/", async (OutgoingProductVM appData, ApplicationContext db) =>
        //{
        //    List<ProductList> existingProducts = ProductList.FromProduct(appData.ProductName);
        //    var OutgoingProduct = new Outgoing(existingProducts, appData.Quantity);

        //    db.Outgoings.Add(OutgoingProduct);
        //    await db.SaveChangesAsync();
        //    return TypedResults.Created($"/api/Outgoing/{OutgoingProduct.Id}", OutgoingProduct);
        //})
        //.WithName("CreateOutgoing")
        //.WithOpenApi();

        //group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, OutgoingProductVM appData, ApplicationContext db) =>
        //{
        //    List<ProductList> existingProducts = ProductList.FromProduct(appData.ProductName);

        //    var affected = await db.Outgoings
        //        .Where(model => model.Id == id)
        //        .ExecuteUpdateAsync(setters => setters
        //          .SetProperty(m => m.ProductName, existingProducts)
        //          .SetProperty(m => m.Quantity, appData.Quantity)
        //        );

        //    return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        //})
        //.WithName("UpdateOutgoing")
        //.WithOpenApi();

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
