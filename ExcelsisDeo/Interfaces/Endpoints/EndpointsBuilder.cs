using ExcelsisDeo.Endpoints;
using System.Reflection;
using ExcelsisDeo.Endpoints.Categories;

namespace ExcelsisDeo.Interfaces.Endpoints
{
    public static class EndpointsBuilder
    {
        public static IEndpointRouteBuilder RegisterEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            types.Where(x => x.IsClass && x.IsAssignableTo(typeof(IEndpoint)))
                .Select(Activator.CreateInstance)
                .Cast<IEndpoint>()
                .ToList()
                .ForEach(x => x!.Configure(endpoints));

            return endpoints;
        }

        public static IServiceCollection RegisterEndpointsHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler, GetAllCategoriesHandler>();

            var requestHandlerInterfaceType = typeof(IRequestHandler<>);

            var assembly = Assembly.GetExecutingAssembly();
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == requestHandlerInterfaceType))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var requestType = handlerType.GetInterfaces()
                    .First(interfaceType => interfaceType.IsGenericType &&
                                            interfaceType.GetGenericTypeDefinition() == requestHandlerInterfaceType)
                    .GetGenericArguments()[0];
            
                var concreteHandlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
                services.AddScoped(concreteHandlerType, handlerType);
            }
            
            return services;
        }
    }
}
