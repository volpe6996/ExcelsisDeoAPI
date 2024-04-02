using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/api/category/",
                async ([FromServices] IRequestHandler getAllCategoriesHandler, CancellationToken cancellationToken)
                    => await (getAllCategoriesHandler.HandleAsync(cancellationToken)))
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);
    }
}

public class GetAllCategoriesHandler : IRequestHandler
{
    private IAppDbContext _appDbContext;

    public GetAllCategoriesHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async ValueTask<IResult> HandleAsync(CancellationToken cancellationToken)
    {
        var categories =
            await _appDbContext.Categories.Select(c => new { c.Id, c.Name }).ToListAsync(cancellationToken);
        
        return categories == null
            ? Results.NoContent()
            : Results.Ok(categories);
    }
}