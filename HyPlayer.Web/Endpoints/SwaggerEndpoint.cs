using HyPlayer.Web.Interfaces;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace HyPlayer.Web.Endpoints;

public class SwaggerEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapOpenApi();
        
        app.MapScalarApiReference();
    }
}