﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using api_inventorymisdss.Domain;
using api_inventorymisdss.Repository;
using api_inventorymisdss.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api_inventorymisdss.Controllers;

public static class ProductsController
{
    private static int GetOverallDemand(ApplicationContext db, long productId)
    {
        return db.Outgoings
            .Where(o => o.OutgoingProductId == productId)
            .Sum(o => o.Quantity);
    }

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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> ([FromRoute] long id, [FromBody] ProductVM appData, ApplicationContext db) =>
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
                  .SetProperty(m => m.LastUpdated, DateTime.UtcNow)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        group.MapGet("/List", async (ApplicationContext db, [FromQuery] string? searchValue) =>
        {
            var productList = await db.Products
            .Select(p => new ProductListVM
            {
                Id = p.Id,
                DisplayName = string.IsNullOrEmpty(p.Measurement)
                ? $"{p.Brand} {p.Name} {p.VariantName}".Trim()
                : $"{p.Brand} {p.Name} {p.VariantName} ({p.Measurement})".Trim(),
                StockCount = p.StockCount
            })
            .ToListAsync();

            productList.Sort(new DisplayNameComparer());

            if (!string.IsNullOrEmpty(searchValue))
            {
                productList = productList
                    .Where(p => p.DisplayName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                productList = productList
                    .Take(10)
                    .ToList();
            }

            return productList;
        })
        .WithName("GetProductList")
        .WithOpenApi();

        group.MapGet("/ListLowStocks", async (ApplicationContext db, int threshold) =>
        {
            var productList = await db.Products
            .Where(p => p.StockCount > 0 && p.StockCount < threshold)
            .OrderBy(p => p.StockCount) //asc
            .Select(p => new ProductListVM
            {
                Id = p.Id,
                DisplayName = string.IsNullOrEmpty(p.Measurement)
                ? $"{p.Brand} {p.Name} {p.VariantName}".Trim()
                : $"{p.Brand} {p.Name} {p.VariantName} ({p.Measurement})".Trim(),
                StockCount = p.StockCount
            })
            .ToListAsync();
            
            return productList;
        })
        .WithName("GetProductListOfLowStocks")
        .WithOpenApi();

        group.MapGet("/ListOutOfStock", async (ApplicationContext db) =>
        {
            var outOfStockProducts = await db.Products
            .Where(p => p.StockCount == 0 || p.StockCount < 0)
            .OrderByDescending(p => p.StockCount) //dsc
            .Select(p => new ProductListVM
            {
                Id = p.Id,
                DisplayName = string.IsNullOrEmpty(p.Measurement)
                ? $"{p.Brand} {p.Name} {p.VariantName}".Trim()
                : $"{p.Brand} {p.Name} {p.VariantName} ({p.Measurement})".Trim(),
                StockCount = p.StockCount
            })
            .ToListAsync();
            
            return outOfStockProducts;
        })
        .WithName("ListOutOfStock")
        .WithOpenApi();

        group.MapGet("/CountLowStock", async (ApplicationContext db, int threshold) =>
        {
            var outOfStockCount = await db.Products
            .Where(p => p.StockCount > 0 && p.StockCount < threshold)
            .CountAsync();

            return outOfStockCount;
        })
        .WithName("CountLowStock")
        .WithOpenApi();

        group.MapGet("/CountOutOfStock", async (ApplicationContext db) =>
        {
            var outOfStockCount = await db.Products
            .Where(p => p.StockCount == 0 || p.StockCount < 0)
            .CountAsync();

            return outOfStockCount;
        })
        .WithName("CountOutOfStock")
        .WithOpenApi();

        group.MapGet("/NumberOfProducts", async (ApplicationContext db) =>
        {
            return await db.Products.CountAsync();
        })
        .WithName("GetNumberOfProducts")
        .WithOpenApi();

        group.MapGet("/", async (ApplicationContext db, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? searchValue) =>
            {
                var query = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (DateTime.TryParse(searchValue, out DateTime searchDate))
                    {
                        // Convert searchDate to UTC to match LastUpdated column
                        searchDate = searchDate.ToUniversalTime();

                        // Apply the search filter for LastUpdated column
                        query = query.Where(p => p.LastUpdated.Equals(searchDate));
                    }
                    else
                    {
                        query = query.Where(p =>
                            p.BarcodeId.Contains(searchValue) ||
                            p.Brand.Contains(searchValue) ||
                            p.Name.Contains(searchValue) ||
                            p.VariantName.Contains(searchValue) ||
                            p.Measurement.Contains(searchValue) ||
                            p.Price.ToString().Contains(searchValue) ||
                            p.StockCount.ToString().Contains(searchValue)
                        );
                    }
                }

                int skip = (page - 1) * pageSize;

                var productList = await query
                .OrderBy(p => p.Brand)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

                return productList;
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

    public class DisplayNameComparer : IComparer<ProductListVM>
    {
        public int Compare(ProductListVM x, ProductListVM y)
        {
            return string.Compare(x.DisplayName, y.DisplayName, StringComparison.Ordinal);
        }
    }
}