using ExcelsisDeo.Interfaces;
using ExcelsisDeo.Persistence;
using ExcelsisDeo.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ExcelsisDeo.Authorization
{
    public class RegisteredUserRoleRequirement : IAuthorizationRequirement { }

    public class RegisteredUserRoleRequirementHandler : AuthorizationHandler<RegisteredUserRoleRequirement>
    {
        private readonly IAppDbContext _dbContext;

        public RegisteredUserRoleRequirementHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRoleRequirement requirement)
        {
            var idClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id)) return;

            if (!await _dbContext.Users.AnyAsync(x => x.Id == id))
                context.Fail(new AuthorizationFailureReason(this, "Registered users only."));
        }
    }
}
