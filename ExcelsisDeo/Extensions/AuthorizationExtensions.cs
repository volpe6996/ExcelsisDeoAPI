using Microsoft.AspNetCore.Authorization;

namespace ExcelsisDeo.Authorization
{
    internal static class AuthorizationExtensions
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RegisteredUserRoleRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, AdminRoleRequirementHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthoriaztionPolicy.RegisteredUser, policy =>
                {
                    policy.AddRequirements(new RegisteredUserRoleRequirement());
                });

                options.AddPolicy(AuthoriaztionPolicy.Admin, policy =>
                {
                    policy.AddRequirements(new AdminRoleRequirement());
                });
            });
        }
    }
}