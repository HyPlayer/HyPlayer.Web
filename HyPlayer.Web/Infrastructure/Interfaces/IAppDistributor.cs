using HyPlayer.Web.Infrastructure.Models;
using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.Infrastructure.Interfaces;

public interface IAppDistributor
{
    public List<ChannelType> BindingChannels { get; }
    public Task<bool> AddDistributionMember(User user, CancellationToken cancellationToken = default);
    public Task<LatestApplicationUpdate?> GetLatestUpdate(ChannelType channelType, CancellationToken cancellationToken = default);
}