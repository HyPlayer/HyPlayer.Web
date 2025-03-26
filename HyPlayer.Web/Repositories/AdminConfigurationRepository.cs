using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models;

namespace HyPlayer.Web.Repositories;

public class AdminConfigurationRepository(IConfiguration configuration) : IAdminRepository
{
    private readonly List<AdministratorModel> _administrators = configuration.GetSection("Administrators")!.Get<List<AdministratorModel>>()!;

    public Task<List<AdministratorModel>> GetAdministratorsAsync()
    {
        return Task.FromResult(_administrators);
    }

    public Task<AdministratorModel?> GetAdministratorAsync(string name)
    {
        return Task.FromResult(_administrators.FirstOrDefault(t=>t.Name == name));
    }
}