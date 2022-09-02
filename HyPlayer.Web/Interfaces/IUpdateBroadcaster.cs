using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Interfaces;

public interface IUpdateBroadcaster
{
    public Task<bool> BroadcastAsync(ChannelType type, List<User> users);
}