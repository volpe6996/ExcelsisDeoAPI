using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Products;

public record UpdateProductRequestBody(Guid productId, string name, string description, decimal price, uint inStockQuantity, Guid categoryId, string photo) : IRequest;

public class UpdateProductEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/api/product/updateProduct", async ([AsParameters] UpdateProductRequestBody updateProductRequestBody,
                    [FromServices] IRequestHandler<UpdateProductRequestBody> updateProductHandler,
                    CancellationToken cancellationToken)
                => await (updateProductHandler.HandleAsync(updateProductRequestBody, cancellationToken)))
            // .RequireAuthorization(AuthorizationPolicy.Admin)
            .WithTags("Products")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductRequestBody>
{
    private IAppDbContext _dbContext;

    public UpdateProductHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<IResult> HandleAsync(UpdateProductRequestBody request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.productId, cancellationToken);
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.categoryId, cancellationToken);
        
        if (product is null || category is null)
            return Results.BadRequest("Produkt lub kategoria nie istnieje");

        product.Name = request.name;
        product.Description = request.description;
        product.Price = request.price;
        product.InStockQuantity = request.inStockQuantity;
        product.CategoryId = request.categoryId;
        product.Photo = request.photo;

        _dbContext.SaveChangesAsync(cancellationToken);
        
        return Results.Ok();
    }
}