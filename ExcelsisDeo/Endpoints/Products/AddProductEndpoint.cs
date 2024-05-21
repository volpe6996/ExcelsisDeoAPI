using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Migrations;
using ExcelsisDeo.Persistence.Entities;
using ExcelsisDeo.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExcelsisDeo.Endpoints.Products;

public record AddProductRequestBody(string name, string description, decimal price, uint inStockQuantity, Guid categoryId, string photo) : IRequest;

public class AddProductEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/api/product/",
                async ([AsParameters] AddProductRequestBody addProductRequestBody,
                        [FromServices] IRequestHandler<AddProductRequestBody> addProductRequestHandler,
                        CancellationToken cancellationToken)
                    => await (addProductRequestHandler.HandleAsync(addProductRequestBody, cancellationToken)))
            // .RequireAuthorization(AuthorizationPolicy.Admin)
            .WithTags("Products")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

public class AddProductRequestHandler : IRequestHandler<AddProductRequestBody>
{
    private IAppDbContext _appDbContext;
    private IValidator<AddProductRequestBody> _validator;

    public AddProductRequestHandler(IAppDbContext appDbContext, IValidator<AddProductRequestBody> validator)
    {
        _appDbContext = appDbContext;
        _validator = validator;
    }

    public async ValueTask<IResult> HandleAsync(AddProductRequestBody request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            return Results.BadRequest("Błąd walidacji");

        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = request.name,
            Description = request.description,
            Price = request.price,
            InStockQuantity = request.inStockQuantity,
            CategoryId = request.categoryId,
            Photo = request.photo,
        };

        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return Results.NoContent();
    }
}

public class AddProductRequestBodyValidator : AbstractValidator<AddProductRequestBody>
{
    public AddProductRequestBodyValidator()
    {
        RuleFor(p => p.name).NotEmpty();
        RuleFor(p => p.description).NotEmpty();
        RuleFor(p => p.price).GreaterThanOrEqualTo(0);
        RuleFor(p => p.categoryId).NotEmpty();
    }
}