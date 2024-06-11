using ExcelsisDeo.Endpoints;
using ExcelsisDeo.Endpoints.Categories;
using ExcelsisDeo.Endpoints.Products;
using FluentValidation;

namespace ExcelsisDeo.Validation
{
    public static class ValidationExtentions
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterRequestBody>, RegisterRequestBodyValidator>();
            services.AddScoped<IValidator<AddProductRequestBody>, AddProductRequestBodyValidator>();
            services.AddScoped<IValidator<AddCategoryRequestBody>, AddCategoryRequestValidator>();
        }
    }
}
