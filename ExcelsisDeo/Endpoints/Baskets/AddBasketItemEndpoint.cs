using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Baskets;

public record AddBasketItemRequestBody(Guid userId, Guid productId, uint quantity) : IRequest;

public class AddBasketItemEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/api/basket/",
                async ([AsParameters] AddBasketItemRequestBody addBasketItemRequestBody,
                        [FromServices] IRequestHandler<AddBasketItemRequestBody> addBasketItemRequestHandler,
                        CancellationToken cancellationToken)
                    => await (addBasketItemRequestHandler.HandleAsync(addBasketItemRequestBody, cancellationToken)))
            .WithTags("Basket")
            // .RequireAuthorization(AuthorizationPolicy.RegisteredUser)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

public class AddBasketItemRequestHandler : IRequestHandler<AddBasketItemRequestBody>
{
    private IAppDbContext _dbContext;

    public AddBasketItemRequestHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<IResult> HandleAsync(AddBasketItemRequestBody request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.userId, cancellationToken);
        
        if (user is null)
            return Results.BadRequest("Użytkownik nie istnieje");
            
        var basket = await _dbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == request.userId, cancellationToken);

        // if user doesnt have their own basket yet
        if (basket is null)
        {
            await _dbContext.Baskets.AddAsync(new Basket() { UserId = request.userId });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        
        var basketId = await _dbContext.Baskets.FirstOrDefaultAsync(b => b.UserId == request.userId, cancellationToken);

        var doesProductExists = await _dbContext.Products.AnyAsync(p => p.Id == request.productId);
        if (!doesProductExists)
            return Results.BadRequest("Produkt nie istnieje");
        
        var isProductAlreadyAdded = await _dbContext.BasketItems.FirstOrDefaultAsync(b => b.ProductId == request.productId && b.BasketId == basketId.Id, cancellationToken);
        if (isProductAlreadyAdded is not null)
        {
            isProductAlreadyAdded.Quantity = request.quantity;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        }
        
        var basketItem = new BasketItem()
        {
            BasketId = basketId.Id,
            ProductId = request.productId,
            Quantity = request.quantity,
        };
        
        await _dbContext.BasketItems.AddAsync(basketItem);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}