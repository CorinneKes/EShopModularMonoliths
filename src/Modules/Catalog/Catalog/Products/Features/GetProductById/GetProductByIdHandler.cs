﻿namespace Catalog.Products.Features.GetProductById;

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        // get product by id using dbContext
        // return result

        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken );

        if (product is null) 
        {
            throw new ProductNotFoundException(query.Id);
        }

        // map product entity to productdto
        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
