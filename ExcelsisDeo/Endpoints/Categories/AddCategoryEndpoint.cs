using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ExcelsisDeo.Endpoints.Categories;

public record AddCategoryRequestBody(string name) : IRequest;

public class AddCategoryEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/api/category/",
                async ([AsParameters] AddCategoryRequestBody addCategoryRequestBody,
                        [FromServices] IRequestHandler<AddCategoryRequestBody> addCategoryRequestHandler,
                        CancellationToken cancellationToken)
                    => await (addCategoryRequestHandler.HandleAsync(addCategoryRequestBody, cancellationToken)))
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}

public class AddCategoryRequestHandler : IRequestHandler<AddCategoryRequestBody>
{
    private IAppDbContext _appDbContext;
    private IValidator<AddCategoryRequestBody> _validator;

    public AddCategoryRequestHandler(IAppDbContext appDbContext, IValidator<AddCategoryRequestBody> validator)
    {
        _appDbContext = appDbContext;
        _validator = validator;
    }

    public async ValueTask<IResult> HandleAsync(AddCategoryRequestBody request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Results.BadRequest("Nazwa nie może być pusta");

        var category = new Category()
        {
            Id = Guid.NewGuid(),
            Name = request.name
        };

        await _appDbContext.Categories.AddAsync(category);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return Results.NoContent();
    }
}

public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequestBody>
{
    public AddCategoryRequestValidator()
    {
        RuleFor(c => c.name).NotEmpty();
    }
}