using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints;

public record LoginUserRequestBody(string email, string password) : IRequest;

public class LoginUserEndpoint : IEndpoint
{
    public void Configure(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/api/login/",
                async ([AsParameters] LoginUserRequestBody loginUserRequestBody,
                        [FromServices] IRequestHandler<LoginUserRequestBody> loginUserRequestHandler,
                        CancellationToken cancellationToken)
                    => await (loginUserRequestHandler.HandleAsync(loginUserRequestBody, cancellationToken)))
            .WithTags("Users")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}

public class LoginUserRequestHandler : IRequestHandler<LoginUserRequestBody>
{
    private IAppDbContext _appDbContext;
    private ITokenProvider _tokenProvider;
    private IPasswordHasher _passwordHasher;

    public LoginUserRequestHandler(IAppDbContext appDbContext, ITokenProvider tokenProvider, IPasswordHasher passwordHasher)
    {
        _appDbContext = appDbContext;
        _tokenProvider = tokenProvider;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<IResult> HandleAsync(LoginUserRequestBody request, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.email, cancellationToken);
        var validationResult = _passwordHasher.ValidatePassword(request.password, user?.PasswordHash);

        if (user == null || !validationResult)
            return Results.BadRequest("Adres email lub hasło są niepoprawne.");

        var token = _tokenProvider.GetAccessToken(user);

        return Results.Ok(new {token, user.Role});
    }
}