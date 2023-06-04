using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;

namespace api_inventorymisdss.Controllers;

public static class OutgoingController
{
    public static void MapOutgoingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Outgoing").WithTags(nameof(Outgoing));

        group.MapPost("/", async (List<OutgoingProductVM> appDataList, ApplicationContext db) =>
        {
            var createdOutgoings = new List<Outgoing>();

            foreach (var appData in appDataList)
            {
                var product = await db.Products.FindAsync(appData.OutgoingProductId);

                if (product != null)
                {
                    var outgoingProduct = new Outgoing(appData.OutgoingProductId, appData.Quantity, appData.DateTimeOutgoing)
                    {
                        ProductPrice = product.Price
                    };

                    if (product.StockCount >= appData.Quantity)
                    {
                        outgoingProduct.TotalPrice = appData.Quantity * product.Price;

                        product.StockCount -= appData.Quantity;
                        product.LastUpdated = DateTime.UtcNow;
                    }

                    if (appData.DateTimeOutgoing != DateTime.MinValue)
                    {
                        outgoingProduct.DateTimeOutgoing = appData.DateTimeOutgoing;
                    }
                    else
                    {
                        outgoingProduct.DateTimeOutgoing = DateTime.UtcNow;
                    }

                    db.Outgoings.Add(outgoingProduct);
                    createdOutgoings.Add(outgoingProduct);
                }
            }

            await db.SaveChangesAsync();

            //if (createdOutgoings.Count > 0)
            //{
            //    return TypedResults.Created($"/api/Outgoing/{createdOutgoings[0].Id}", createdOutgoings);
            //}
            //return TypedResults.NotFound();
        })
        .WithName("CreateOutgoingEntry")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (long id, OutgoingProductVM appData, ApplicationContext db) =>
        {
            var preOutgoing = await db.Outgoings.FindAsync(id);
            var product = await db.Products.FindAsync(appData.OutgoingProductId);

            var affected = await db.Outgoings
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.OutgoingProductId, appData.OutgoingProductId)
                  .SetProperty(m => m.Quantity, appData.Quantity)
                  .SetProperty(m => m.ProductPrice, product.Price)
                  .SetProperty(m => m.TotalPrice, appData.Quantity * product.Price)
                  .SetProperty(m => m.DateTimeOutgoing, appData.DateTimeOutgoing)
                  .SetProperty(m => m.LastUpdated, DateTime.UtcNow)
                );

            int diffQuantity = preOutgoing.Quantity - appData.Quantity;
            product.StockCount += diffQuantity;
            product.LastUpdated = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOutgoingEntry")
        .WithOpenApi();

        group.MapGet("/List", async (ApplicationContext db) =>
        {
            var outgoingList = await db.Outgoings.Select(o => new OutgoingListVM
            {
                Id = o.Id,
                Quantity = o.Quantity,
                ProductName = $"{o.Product.Brand} {o.Product.Name} {o.Product.VariantName} ({o.Product.Measurement})",
                ProductPrice = o.ProductPrice,
                TotalPrice = o.TotalPrice,
                DateTimeOutgoing = o.DateTimeOutgoing,
                LastUpdated = o.LastUpdated
            }).ToListAsync();

            return outgoingList;
        })
        .WithName("GetOutgoingList")
        .WithOpenApi();

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
        .WithName("GetOutgoingProductById")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (long id, ApplicationContext db) =>
        {
            var affected = await db.Outgoings
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOutgoingEntry")
        .WithOpenApi();
    }
}