using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.AppDistributors;

public class SelfHostDistributor(IRepository<Release, Guid> releasesRepository) : IAppDistributor
{
    public string Name => "SelfHost";
    private readonly IRepository<Release, Guid> _releasesRepository = releasesRepository;

    public List<ChannelType> BindingChannels => [ChannelType.Canary, ChannelType.Release, ChannelType.Dogfood];

    public async Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var release = (await releasesRepository.GetQueryableEntitiesAsync(cancellationToken))
            .Where(t=>t.Channels.Contains(channelType))
            .OrderByDescending(t => t.ReleaseDate)
            .FirstOrDefault();
        return release == null ? null : new LatestApplicationUpdate
        {
            Version = release.Version,
            Date = release.ReleaseDate,
            Mandatory = false,
            DownloadUrl = null,
            UpdateLog = release.ReleaseNotes,
            Size = 0
        };
    }
}