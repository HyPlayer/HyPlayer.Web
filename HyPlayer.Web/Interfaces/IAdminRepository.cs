using HyPlayer.Web.Models;

namespace HyPlayer.Web.Interfaces;

public interface IAdminRepository
{
    public Task<List<AdministratorModel>> GetAdministratorsAsync();
    public Task<AdministratorModel?> GetAdministratorAsync(string name);
}