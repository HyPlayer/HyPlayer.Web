﻿using HyPlayer.Web.AppDistributors;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Endpoints;

public class AppDistributorEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IAppDistributor, AppCenterDistributor>();
        services.AddScoped<IAppDistributor, MicrosoftStoreDistributor>();
        services.AddScoped<IAppDistributor, GithubDistributor>();
        services.AddScoped<IAppDistributor, SelfHostDistributor>();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapGet("/appDistributors", GetAppDistributors);
    }
    

    private Task<IResult> GetAppDistributors(IEnumerable<IAppDistributor> distributors)
    {
        return Task.FromResult(Results.Json(distributors.Select(t => new
        {
            name = t.Name,
            channels = t.BindingChannels
        })));
    }
}