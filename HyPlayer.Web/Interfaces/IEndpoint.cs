namespace HyPlayer.Web.Interfaces;

public interface IEndpoint
{
    public void ConfigureServices(IServiceCollection services);
    public void ConfigureEndpoint(WebApplication app);
}