using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthorizationPolicy = ExcelsisDeo.Authorization.AuthorizationPolicy;

namespace ExcelsisDeo.Endpoints.Baskets;
public record GetUserBasketRequestBody(Guid userId) : IRequest;

public class GetUserBasketEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/api/basket/",
                async ([AsParameters] GetUserBasketRequestBody getUserBasketRequestBody,
                        [FromServices] IRequestHandler<GetUserBasketRequestBody> getUserBasketRequestHandler,
                        CancellationToken cancellationToken)
                    => await (getUserBasketRequestHandler.HandleAsync(getUserBasketRequestBody, cancellationToken)))
            .WithTags("Basket")
            // .RequireAuthorization(AuthorizationPolicy.RegisteredUser)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}

public class GetUserBasketRequestHandler : IRequestHandler<GetUserBasketRequestBody>
{
    private IAppDbContext _dbContext;

    public GetUserBasketRequestHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<IResult> HandleAsync(GetUserBasketRequestBody request, CancellationToken cancellationToken)
    { 
        var basket = await _dbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == request.userId, cancellationToken);

        // if user doesnt have their own basket yet
        if (basket is null)
        {
            await _dbContext.Baskets.AddAsync(new Basket() { UserId = request.userId });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok("Koszyk utworzony");
        }
        
        var basketItems = _dbContext.BasketItems.Where(b => b.BasketId == basket.Id).ToList();

        return Results.Ok(basketItems);
    }
}
