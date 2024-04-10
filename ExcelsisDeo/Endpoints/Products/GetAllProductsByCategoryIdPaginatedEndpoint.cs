using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExcelsisDeo.Endpoints.Products;

public record GetAllProductsByCategoryIdPaginatedRequestBody(string categoryId, int pageNumber) : IRequest;

public class GetAllProductsByCategoryIdPaginatedEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/api/product/getAllByCategoryId/",
                async ([AsParameters] GetAllProductsByCategoryIdPaginatedRequestBody getAllProductsByCategoryIdPaginatedRequestBody,
                        [FromServices] IRequestHandler<GetAllProductsByCategoryIdPaginatedRequestBody> getAllProductsByCategoryIdPaginatedRequestHandler,
                        CancellationToken cancellationToken)
                    => await (getAllProductsByCategoryIdPaginatedRequestHandler.HandleAsync(getAllProductsByCategoryIdPaginatedRequestBody, cancellationToken)))
            // .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

public class GetAllProductsByCategoryIdPaginatedRequestHandler
    : IRequestHandler<GetAllProductsByCategoryIdPaginatedRequestBody>
{
    private IAppDbContext _appDbContext;

    public GetAllProductsByCategoryIdPaginatedRequestHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async ValueTask<IResult> HandleAsync(GetAllProductsByCategoryIdPaginatedRequestBody request, CancellationToken cancellationToken)
    {
        var pageSize = 5;
        var toSkip = (request.pageNumber - 1) * pageSize;
        
        Guid categoryId = Guid.Empty;
        if (!Guid.TryParse(request.categoryId, out categoryId))
            return Results.BadRequest("Podane Id to nie Guid");
        
        var productsByCategoryId =
            _appDbContext.Products.Where(p => p.CategoryId == Guid.Parse(request.categoryId)).ToList();

        var pageCount = Math.Ceiling(productsByCategoryId.Count() / (decimal)pageSize);

        var productsByCategoryIdPaginated = productsByCategoryId
            .Skip(toSkip)
            .Take(pageSize)
            .ToList();
        

        return Results.Ok(new PaginatedList<Product>(pageCount, request.pageNumber, productsByCategoryIdPaginated));
    }
}
