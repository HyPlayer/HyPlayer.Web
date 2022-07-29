using HyPlayer.Web.Infrastructure.Models;

namespace HyPlayer.Web.Infrastructure.Interfaces;

public interface IAdminRepository
{
    public Task<List<AdministratorModel>> GetAdministratorsAsync();
    public Task<AdministratorModel?> GetAdministratorAsync(string name);
}