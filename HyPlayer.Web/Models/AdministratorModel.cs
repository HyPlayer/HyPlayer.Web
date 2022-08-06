using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Models;

public class AdministratorModel
{
    public required string Name { get; set; }
    public required string Mail { get; set; }
    public required string Password { get; set; }
    public required ChannelType Subscription { get; set; }
}