using System.Reflection;
using HyPlayer.Web.Interfaces;

namespace HyPlayer.Web.Extensions;

public static class EndpointExtension
{
    public static void AddEndpointsByAssembly(this IServiceCollection services, params Assembly[] assemblyCollection)
    {
        var allEndpoints = new List<IEndpoint>();
        foreach (var assembly in assemblyCollection)
        {
            foreach (var endpointType in assembly.ExportedTypes
                         .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface))
            {
                if (Activator.CreateInstance(endpointType) is not IEndpoint endpoint) continue;
                allEndpoints.Add(endpoint);
                services.AddSingleton(typeof(IEndpoint), endpoint);
            }
        }
        
        foreach (var endpoint in allEndpoints)
        {
            endpoint.ConfigureServices(services);
        }
    }

    public static void UseEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.ConfigureEndpoint(app);
        }
    }
}