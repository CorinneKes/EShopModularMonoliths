﻿using Microsoft.EntityFrameworkCore;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;


public record GetProductsQuery(PaginationRequest PaginationRequest)
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
       // get products using dbContext
       // return result

       var pageIndex = query.PaginationRequest.PageIndex;
       var pageSize = query.PaginationRequest.PageSize;

       var totalcount = await dbContext.Products.LongCountAsync(cancellationToken);

       var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // map product entitty to productdto using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex,
                pageSize,
                totalcount,
                productDtos)
            );
    }
}
