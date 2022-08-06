using HyPlayer.Web.Models;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Interfaces;

public interface IAppDistributor
{
    public string Name { get; }
    public List<ChannelType> BindingChannels { get; }
    public Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default);
    public Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType, CancellationToken cancellationToken = default);
}