using HyPlayer.Web.AppDistributors;
using HyPlayer.Web.Infrastructure.Interfaces;

namespace HyPlayer.Web.Endpoints;

public class AppDistributorEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAppDistributor, AppCenterDistributor>();
        services.AddSingleton<IAppDistributor, MicrosoftStoreDistributor>();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        
    }
}