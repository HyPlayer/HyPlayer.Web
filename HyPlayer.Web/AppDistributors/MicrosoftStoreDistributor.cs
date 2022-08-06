using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.AppDistributors;

public class MicrosoftStoreDistributor : IAppDistributor
{
    public string Name => "MSStore";

    public List<ChannelType> BindingChannels => new()
        { ChannelType.StoreBeta, ChannelType.StoreRelease };

    public Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<LatestApplicationUpdate?>(null);
    }

}