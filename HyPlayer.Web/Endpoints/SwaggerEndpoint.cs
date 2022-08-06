using HyPlayer.Web.Interfaces;
using Microsoft.OpenApi.Models;

namespace HyPlayer.Web.Endpoints;

public class SwaggerEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc($"v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}", new OpenApiInfo()
            {
                Title = typeof(SwaggerEndpoint).Assembly.GetName().Name,
                Version = $"v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}"
            });
        });
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint($"/swagger/v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}/swagger.json", typeof(SwaggerEndpoint).Assembly.GetName().Name));
    }
}