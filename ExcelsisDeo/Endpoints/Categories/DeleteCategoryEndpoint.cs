using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints.Categories;

public record DeleteCategoryRequestBody(Guid id) : IRequest;

public class DeleteCategoryEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapDelete("/api/category/{id:guid}",
                async ([AsParameters] DeleteCategoryRequestBody deleteCategoryRequestBody,
                        [FromServices] IRequestHandler<DeleteCategoryRequestBody> deleteCategoryRequestHandler,
                        CancellationToken cancellationToken)
                    => await (deleteCategoryRequestHandler.HandleAsync(deleteCategoryRequestBody, cancellationToken)))
            .WithTags("Categoires")
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
    }
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequestBody>
{
    private IAppDbContext _appDbContext;

    public DeleteCategoryRequestHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async ValueTask<IResult> HandleAsync(DeleteCategoryRequestBody request, CancellationToken cancellationToken)
    {
        var deletedCategoriesCount 
            = await _appDbContext.Categories.Where(c => c.Id.Equals(request.id))
                .ExecuteDeleteAsync(cancellationToken);

        return deletedCategoriesCount == 0
            ? Results.NotFound($"Kategoria o ID: {request.id} nie istnieje.")
            : Results.NoContent();
    }
}

// public class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryRequestBody>
// {
//     public DeleteCategoryRequestValidator()
//     {
//         RuleFor(c => c.id).
//     }
// }