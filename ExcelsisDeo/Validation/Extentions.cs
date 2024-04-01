using ExcelsisDeo.Endpoints;
using FluentValidation;

namespace ExcelsisDeo.Validation
{
    public static class Extentions
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterRequestBody>, RegisterRequestBodyValidator>();
        }
    }
}
