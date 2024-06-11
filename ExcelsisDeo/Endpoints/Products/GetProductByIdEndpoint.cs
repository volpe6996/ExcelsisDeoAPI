using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Products;

public record GetProductByIdRequestBody(string id) : IRequest;

public class GetProductByIdEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/api/product/",
                async ([AsParameters] GetProductByIdRequestBody getProductByIdRequestBody,
                        [FromServices] IRequestHandler<GetProductByIdRequestBody> getProductByIdRequestHandler,
                        CancellationToken cancellationToken)
                    => await (getProductByIdRequestHandler.HandleAsync(getProductByIdRequestBody, cancellationToken)))
            // .RequireAuthorization(AuthorizationPolicy.Admin)
            .WithTags("Products")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}

public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequestBody>
{
    private IAppDbContext _appDbContext;

    public GetProductByIdRequestHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async ValueTask<IResult> HandleAsync(GetProductByIdRequestBody request, CancellationToken cancellationToken)
    {
        Guid productId;
        var isGuid = Guid.TryParse(request.id, out productId);
        
        if (request.id.Equals(null) || request.id.Equals("") || !isGuid)
            return Results.BadRequest("Id nie może być null or empty");

        var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(Guid.Parse(request.id)));

        return product.Equals(null)
            ? Results.NotFound($"Produkt o ID: {request.id} nie istnieje.")
            : Results.Ok(product);
    }
}
