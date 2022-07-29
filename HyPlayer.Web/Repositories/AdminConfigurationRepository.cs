using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models;

namespace HyPlayer.Web.Repositories;

public class AdminConfigurationRepository : IAdminRepository
{
    private readonly List<AdministratorModel> _administrators;

    public AdminConfigurationRepository(IConfiguration configuration)
    {
        _administrators = configuration.GetSection("Administrators")!.Get<List<AdministratorModel>>()!;
    }

    public async Task<List<AdministratorModel>> GetAdministratorsAsync()
    {
        return _administrators;
    }

    public async Task<AdministratorModel?> GetAdministratorAsync(string name)
    {
        return _administrators.FirstOrDefault(t=>t.Name == name);
    }
}