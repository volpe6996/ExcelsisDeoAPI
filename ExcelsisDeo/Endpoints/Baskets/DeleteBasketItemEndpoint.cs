using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Baskets;

public record DeleteBasketItemRequestBody(Guid userId, Guid productId) : IRequest;

public class DeleteBasketItemEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapDelete("/api/basket/",
                async ([AsParameters] DeleteBasketItemRequestBody deleteBasketItemRequestBody,
                        [FromServices] IRequestHandler<DeleteBasketItemRequestBody> deleteBasketItemRequestHandler,
                        CancellationToken cancellationToken)
                    => await (deleteBasketItemRequestHandler.HandleAsync(deleteBasketItemRequestBody, cancellationToken)))
            .WithTags("Basket")
            // .RequireAuthorization(AuthorizationPolicy.RegisteredUser)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}

public class DeleteBasketItemRequestHandler : IRequestHandler<DeleteBasketItemRequestBody>
{
    private IAppDbContext _dbContext;

    public DeleteBasketItemRequestHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<IResult> HandleAsync(DeleteBasketItemRequestBody request, CancellationToken cancellationToken)
    {
        var basket = await _dbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == request.userId);

        if (basket is null)
            return Results.BadRequest("Użytkownik nie istnieje");
        
        var basketItem = _dbContext
            .BasketItems
            .Where(b => b.BasketId == basket.Id && b.ProductId == request.productId).ToList();
        
        if (basketItem.Count == 0)
            return Results.BadRequest("Produkt nie istnieje");

        _dbContext.BasketItems.RemoveRange(basketItem);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}
