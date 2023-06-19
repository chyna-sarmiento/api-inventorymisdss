﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

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
                        product.StockCount -= appData.Quantity;
                    }
                    else
                    {
                        product.StockCount = 0;
                    }

                    outgoingProduct.TotalPrice = appData.Quantity * product.Price;
                    product.LastUpdated = DateTime.UtcNow;

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

        group.MapGet("/List", async (ApplicationContext db, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? searchValue) =>
        {
            var query = db.Outgoings.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (DateTime.TryParse(searchValue, out DateTime searchDate))
                {
                    // Convert searchDate to UTC to match LastUpdated column
                    searchDate = searchDate.ToUniversalTime();

                    // Apply the search filter for LastUpdated column
                    query = query.Where(o => o.DateTimeOutgoing.Equals(searchDate) || o.LastUpdated.Equals(searchDate));
                }
                else
                {
                    query = query.Where(o =>
                    o.Id.ToString().Contains(searchValue) ||
                    o.Quantity.ToString().Contains(searchValue) ||
                    o.OutgoingProductId.ToString().Contains(searchValue) ||
                    o.Product.Brand.Contains(searchValue) ||
                    o.Product.Name.Contains(searchValue) ||
                    o.Product.VariantName.Contains(searchValue) ||
                    o.Product.Measurement.Contains(searchValue) ||
                    o.ProductPrice.ToString().Contains(searchValue) ||
                    o.TotalPrice.ToString().Contains(searchValue)
                    );
                }
            }

            int skip = (page - 1) * pageSize;

            var outgoingList = await query
            .OrderBy(o => o.DateTimeOutgoing)
            .Skip(skip)
            .Take(pageSize)
            .Select(o => new OutgoingListVM
            {
                Id = o.Id,
                Quantity = o.Quantity,
                OutgoingProductId = o.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(o.Product.Measurement)
                ? $"{o.Product.Brand} {o.Product.Name} {o.Product.VariantName}".Trim()
                : $"{o.Product.Brand} {o.Product.Name} {o.Product.VariantName} ({o.Product.Measurement})".Trim(),
                ProductPrice = o.ProductPrice,
                TotalPrice = o.TotalPrice,
                DateTimeOutgoing = o.DateTimeOutgoing,
                LastUpdated = o.LastUpdated
            }).ToListAsync();

            return outgoingList;
        })
        .WithName("GetOutgoingList")
        .WithOpenApi();

        group.MapGet("/NumberOfEntries", async (ApplicationContext db) =>
        {
            return await db.Outgoings.CountAsync();
        })
        .WithName("GetOutgoingNumberOfEntries")
        .WithOpenApi();

        group.MapGet("/", async (ApplicationContext db) =>
        {
            var outgoings = await db.Outgoings.ToListAsync();
            outgoings.Sort(new DateTimeOutgoingComparer());

            return outgoings;
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

        group.MapGet("/EarliestDate", async (ApplicationContext db) =>
        {
            var earliestDate = await db.Outgoings.MinAsync(o => o.DateTimeOutgoing); // Retrieve the earliest date

            return earliestDate;
        })
        .WithName("GetOutgoingsEarliestDate")
        .WithOpenApi();

        group.MapGet("/LatestDate", async (ApplicationContext db) =>
        {
            var latestDate = await db.Outgoings.MaxAsync(o => o.DateTimeOutgoing); // Retrieve the latest date

            return latestDate;
        })
        .WithName("GetOutgoingsLatestDate")
        .WithOpenApi();

        #region Forecast Data
        group.MapGet("/ForecastData", async (ApplicationContext db) =>
        {
            var outgoingDemand = await db.Outgoings
            .GroupBy(o => o.OutgoingProductId)
            .Select(g => new OutgoingMonthlyForecastVM
            {
                OutgoingProductId = g.Key,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Count()
            })
        .ToListAsync();

            return outgoingDemand;
        })
        .WithName("GetOutgoingForecastData")
        .WithOpenApi();

        group.MapGet("/MonthlyForecastData", async (ApplicationContext db) =>
        {
            var outgoingMonthlyDemand = await db.Outgoings
            .GroupBy(o => new { o.OutgoingProductId, o.DateTimeOutgoing.Year, o.DateTimeOutgoing.Month})
            .Select(g => new OutgoingMonthlyForecastVM
            {
                OutgoingYear = g.Key.Year,
                OutgoingMonth = g.Key.Month,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToListAsync();

            return outgoingMonthlyDemand;
        })
        .WithName("GetListOutgoingMonthlyData")
        .WithOpenApi();

        group.MapGet("/MonthlyForecastData/{year}/{month}", async (ApplicationContext db, int year, int month) =>
        {
            var outgoingMonthlyDemand = await db.Outgoings
            .GroupBy(o => new { o.OutgoingProductId, o.DateTimeOutgoing.Year, o.DateTimeOutgoing.Month})
            .Where(g => g.Key.Month == month && g.Key.Year == year)
            .Select(g => new OutgoingMonthlyForecastVM
            {
                OutgoingYear = g.Key.Year,
                OutgoingMonth = g.Key.Month,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),

                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToListAsync();

            return outgoingMonthlyDemand;
        })
        .WithName("GetDataOutgoingMonth")
        .WithOpenApi();

        group.MapGet("/WeeklyForecastData", async (ApplicationContext db) =>
        {
            var outgoingData = await db.Outgoings
            .Select(o => new
            {
                o.OutgoingProductId,
                o.Quantity,
                o.DateTimeOutgoing.Year,
                o.DateTimeOutgoing,
                o.Product
            })
            .ToListAsync();

            var outgoingWeeklyDemand = outgoingData
            .GroupBy(o => new { o.OutgoingProductId, o.Year, Week = ISOWeek.GetWeekOfYear(o.DateTimeOutgoing) })
            .Select(g => new OutgoingWeeklyForecastVM
            {
                OutgoingYear = g.Key.Year,
                OutgoingWeek = g.Key.Week,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToList();

            return outgoingWeeklyDemand;
        })
        .WithName("GetListOutgoingWeeklyData")
        .WithOpenApi();

        group.MapGet("/WeeklyForecastData/{year}/{week}", async (ApplicationContext db, int year, int week) =>
        {
            var outgoingData = await db.Outgoings
            .Select(o => new
            {
                o.OutgoingProductId,
                o.Quantity,
                o.DateTimeOutgoing.Year,
                o.DateTimeOutgoing,
                o.Product
            })
            .ToListAsync();

            var outgoingWeeklyDemand = outgoingData
            .GroupBy(o => new { o.OutgoingProductId, o.Year, Week = ISOWeek.GetWeekOfYear(o.DateTimeOutgoing)})
            .Where(g => g.Key.Week == week && g.Key.Year == year)
            .Select(g => new OutgoingWeeklyForecastVM
            {
                OutgoingYear = g.Key.Year,
                OutgoingWeek = g.Key.Week,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToList();

            return outgoingWeeklyDemand;
        })
        .WithName("GetDataOutgoingWeek")
        .WithOpenApi();

        group.MapGet("/DailyForecastData", async (ApplicationContext db) =>
        {
            var outgoingDailyDemand = await db.Outgoings
            .GroupBy(o => new { o.OutgoingProductId, o.DateTimeOutgoing.Date })
            .Select(g => new OutgoingDailyForecastVM
            {
                OutgoingDate = g.Key.Date,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToListAsync();

            return outgoingDailyDemand;
        })
        .WithName("GetListOutgoingDailyData")
        .WithOpenApi();

        group.MapGet("/DailyForecastData/{date}", async (ApplicationContext db, DateTime date) =>
        {
            var outgoingDailyDemand = await db.Outgoings
            .Where(o => o.DateTimeOutgoing.Date == date.Date)
            .GroupBy(o => new { o.OutgoingProductId, o.DateTimeOutgoing.Date })
            .Select(g => new OutgoingDailyForecastVM
            {
                OutgoingDate = g.Key.Date,
                OutgoingProductId = g.Key.OutgoingProductId,
                ProductName = string.IsNullOrEmpty(g.First().Product.Measurement)
                ? $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName}".Trim()
                : $"{g.First().Product.Brand} {g.First().Product.Name} {g.First().Product.VariantName} ({g.First().Product.Measurement})".Trim(),
                OutgoingDemandVolume = g.Sum(o => o.Quantity)
            }).ToListAsync();

            return outgoingDailyDemand;
        })
        .WithName("GetOutgoingDay")
        .WithOpenApi();
        #endregion
    }
    public class DateTimeOutgoingComparer : IComparer<Outgoing>
    {
        public int Compare(Outgoing x, Outgoing y)
        {
            return DateTime.Compare(x.DateTimeOutgoing, y.DateTimeOutgoing);
        }
    }
}