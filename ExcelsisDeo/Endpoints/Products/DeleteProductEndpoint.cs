using System.Runtime.InteropServices.ComTypes;
using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Products;

public record DeleteProductRequestBody(Guid id) : IRequest;

public class DeleteProductEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapDelete("/api/product/{id:guid}",
                async ([AsParameters] DeleteProductRequestBody deleteProductRequestBody,
                        [FromServices] IRequestHandler<DeleteProductRequestBody> deleteProductRequestHandler,
                        CancellationToken cancellationToken)
                    => await (deleteProductRequestHandler.HandleAsync(deleteProductRequestBody, cancellationToken)))
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);
    }
}

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequestBody>
{
    private IAppDbContext _appDbContext;

    public DeleteProductRequestHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async ValueTask<IResult> HandleAsync(DeleteProductRequestBody request, CancellationToken cancellationToken)
    {
        var deletedProductsCount 
            = await _appDbContext.Products
                .Where(p => p.Id.Equals(request.id))
                .ExecuteDeleteAsync(cancellationToken);

        return deletedProductsCount == 0
            ? Results.NotFound($"Produkt o ID: {request.id} nie istnieje")
            : Results.NoContent();
    }
}
