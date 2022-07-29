using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models;
using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.AppDistributors;

public class MicrosoftStoreDistributor : IAppDistributor
{
    public List<ChannelType> BindingChannels => new()
        { ChannelType.StoreBeta, ChannelType.StoreRelease };

    public async Task<bool> AddDistributionMember(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<LatestApplicationUpdate> GetLatestUpdate(ChannelType channelType, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}