using ExcelsisDeo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Persistence
{
    internal static class Extensions
    {
        internal static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IAppDbContext, AppDbContext>();
        }
    }
}
