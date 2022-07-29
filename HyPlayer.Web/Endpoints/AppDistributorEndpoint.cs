using HyPlayer.Web.AppDistributors;
using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.Endpoints;

public class AppDistributorEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAppDistributor, AppCenterDistributor>();
        services.AddSingleton<IAppDistributor, MicrosoftStoreDistributor>();
        services.AddSingleton<IAppDistributor, GithubDistributor>();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        
    }
}