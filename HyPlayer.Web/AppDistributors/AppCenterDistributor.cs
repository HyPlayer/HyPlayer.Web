using System.Text.Json.Serialization;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models;
using HyPlayer.Web.Models.DbModels;
using Microsoft.Extensions.Caching.Hybrid;

namespace HyPlayer.Web.AppDistributors;

public class AppCenterDistributor : IAppDistributor
{
    public AppCenterDistributor(IConfiguration configuration, ILogger<AppCenterDistributor> logger,
        HybridCache hybridCache)
    {
        _logger = logger;
        _hybridCache = hybridCache;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-API-Token",
            configuration.GetValue<string>("Distributors:AppCenter:UserApiToken"));
        _httpClient.BaseAddress = new Uri("https://api.appcenter.ms/");
        _ownerName = configuration.GetValue<string>("Distributors:AppCenter:OwnerName")!;
        _appName = configuration.GetValue<string>("Distributors:AppCenter:AppName")!;
    }

    public string Name => "AppCenter";

    public List<ChannelType> BindingChannels => new()
        { ChannelType.AppCenterRelease, ChannelType.AppCenterCanary };

    private readonly HttpClient _httpClient;
    private readonly string _ownerName;
    private readonly string _appName;
    private readonly ILogger<AppCenterDistributor> _logger;
    private readonly HybridCache _hybridCache;

    private Dictionary<ChannelType, string> ChannelTypeToName => new()
    {
        { ChannelType.AppCenterRelease, "Release" },
        { ChannelType.AppCenterCanary, "Canary" }
    };

    private class ErrorResponse
    {
        public class ErrorMessage
        {
            [JsonPropertyName("code")] public string? Code { get; set; }

            [JsonPropertyName("message")] public string? Message { get; set; }
        }

        [JsonPropertyName("error")] public ErrorMessage? Error { get; set; }
    }

    public class ReleaseInfo
    {
        [JsonPropertyName("version")] public required string? Version { get; set; }

        [JsonPropertyName("uploaded_at")] public required DateTime UploadDate { get; set; }

        [JsonPropertyName("mandatory_update")] public required bool IsMandatory { get; set; }

        [JsonPropertyName("size")] public required int Size { get; set; }

        [JsonPropertyName("download_url")] public required string DownloadUrl { get; set; }

        [JsonPropertyName("release_notes")] public required string ReleaseNotes { get; set; }
    }


    public async Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.PostAsJsonAsync(
            $"/v0.1/apps/{_ownerName}/{_appName}/distribution_groups/{ChannelTypeToName[user.ChannelType]}/members",
            new
            {
                user_emails = new List<string>()
                {
                    user.Email
                }
            }, cancellationToken: cancellationToken);
        if (result.IsSuccessStatusCode) return true;

        var errorBody = await result.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
        _logger.LogError("Failed to add {UserName}({Mail}) to distribution group {GroupName}, message is {message}",
            user.UserName, user.Email, ChannelTypeToName[user.ChannelType], errorBody?.Error?.Message);
        return false;
    }


    public async Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var latestReleaseInfo = await _hybridCache.GetOrCreateAsync($"AppCenter_Channel{ChannelTypeToName[channelType]}",
                async token => await GetLatestReleaseInfoAsync(channelType, token), token: cancellationToken)
            .ConfigureAwait(false);
        return new LatestApplicationUpdate
        {
            Version = latestReleaseInfo?.Version!,
            Date = latestReleaseInfo?.UploadDate ?? DateTime.MinValue,
            Mandatory = latestReleaseInfo?.IsMandatory ?? false,
            DownloadUrl = latestReleaseInfo?.DownloadUrl!,
            UpdateLog = latestReleaseInfo?.ReleaseNotes!,
            Size = latestReleaseInfo?.Size ?? -1
        };
    }

    public async Task<ReleaseInfo> GetLatestReleaseInfoAsync(ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var result =
            await _httpClient.GetAsync(
                $"/v0.1/apps/{_ownerName}/{_appName}/distribution_groups/{ChannelTypeToName[channelType]}/releases/latest",
                cancellationToken);
        result.EnsureSuccessStatusCode();
        var releaseInfos = await result.Content.ReadFromJsonAsync<ReleaseInfo>(cancellationToken: cancellationToken);
        return releaseInfos!;
    }
}