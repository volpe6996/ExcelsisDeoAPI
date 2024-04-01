using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence.Entities;
using ExcelsisDeo.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Endpoints
{
    public record RegisterRequestBody(string fullName, string email, string phoneNumber, string password) : IRequest;

    public class RegisterUserEndpoint : IEndpoint
    {
        public void Configure(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/api/register/",
                    async ([FromBody] RegisterRequestBody registerRequestBody,
                            [FromServices] IRequestHandler<RegisterRequestBody> registerRequestHandler,
                            CancellationToken cancellationToken)
                        => await (registerRequestHandler.HandleAsync(registerRequestBody, cancellationToken)))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status409Conflict);
        }
    }

    public class RegisterRequestHandler : IRequestHandler<RegisterRequestBody>
    {
        private IAppDbContext _appDbContext;
        private IValidator<RegisterRequestBody> _validator;
        private IPasswordHasher _passwordHasher;

        public RegisterRequestHandler(IAppDbContext appDbContext, IPasswordHasher passwordHasher, IValidator<RegisterRequestBody> validator)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async ValueTask<IResult> HandleAsync(RegisterRequestBody request, CancellationToken cancellationToken)
        {
            var validationResult = await  _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return Results.BadRequest(ValidationErrorPraser.Prase(validationResult.Errors));

            var isEmailAlreadyExists = await _appDbContext.Users.AnyAsync(u => u.Email == request.email, cancellationToken);

            if (isEmailAlreadyExists)
                return Results.Conflict("Konto o podanym adresie e-mail istnieje.");
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.fullName,
                Email = request.email,
                PhoneNumber = request.phoneNumber,
                PasswordHash = _passwordHasher.HashPassword(request.password),
            };

            await _appDbContext.Users.AddAsync(user, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
    }

    public class RegisterRequestBodyValidator : AbstractValidator<RegisterRequestBody>
    {
        public RegisterRequestBodyValidator()
        {
            RuleFor(u => u.email)
                .MaximumLength(48)
                .Must(e => e.Any(IsAtSign))
                .WithMessage("Niepoprawny adres e-mail");
            
            RuleFor(u => u.phoneNumber)
                .Length(9)
                .Must(pn => pn.All(char.IsDigit))
                .WithMessage("Niepoprawny numer telefonu");
            
            RuleFor(u => u.password)
                .MinimumLength(8)
                .Must(password => password.Any(char.IsUpper))
                .Must(password => password.Any(char.IsLower))
                .Must(password => password.Any(char.IsDigit))
                .Must(password => password.Any(IsSpecialChar))
                .WithMessage("Zbyt słabe hasło");
        }

        private bool IsSpecialChar(char c)
        {
            return !char.IsLetterOrDigit(c);
        }

        private bool IsAtSign(char c)
        {
            return c == '@';
        }
    }
}
