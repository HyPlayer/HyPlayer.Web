using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.Infrastructure.Models;

public class AdministratorModel
{
    public required string Name { get; set; }
    public required string Mail { get; set; }
    public required string Password { get; set; }
    public required ChannelType Subscription { get; set; }
}