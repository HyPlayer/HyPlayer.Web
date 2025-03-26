using System.Net.Mail;
using System.Text.Json.Serialization;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models;
using HyPlayer.Web.Models.DbModels;
using Microsoft.Extensions.Caching.Hybrid;

namespace HyPlayer.Web.AppDistributors;

public class GithubDistributor(
    IConfiguration configuration,
    IEmailService emailService,
    IEmailTemplateProvider emailTemplateProvider,
    HttpClient httpClient,
    HybridCache hybridCache)
    : IAppDistributor
{
    private readonly string _org = configuration.GetValue<string>("Distributors:Github:OrganizationName")!;
    private readonly string _proj = configuration.GetValue<string>("Distributors:Github:ProjectName")!;
    private readonly string _tag = configuration.GetValue<string>("Distributors:Github:TagName")!;

    private class GithubReleaseResponse
    {
        [JsonPropertyName("name")] public required string Name { get; set; }

        [JsonPropertyName("html_url")] public required string HtmlUrl { get; set; }

        [JsonPropertyName("prerelease")] public required bool IsPrerelease { get; set; }

        [JsonPropertyName("published_at")] public required DateTime PublishedAt { get; set; }

        [JsonPropertyName("body")] public required string Body { get; set; }
    }

    public string Name => "Github";
    public List<ChannelType> BindingChannels => new() { ChannelType.GithubNightly };

    public async Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default)
    {
        var template = await emailTemplateProvider.GetTemplateAsync("GithubNightly", cancellationToken);
        return await emailService.SendMailToAsync(user.Email, "[HyPlayer] 内测申请成功",
            template.Replace("{USERNAME}", user.UserName), cancellationToken: cancellationToken);
    }

    private async Task<GithubReleaseResponse?> GetReleaseResponse(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<GithubReleaseResponse?>(
            $"https://api.github.com/repos/{_org}/{_proj}/releases/tags/{_tag}", cancellationToken: cancellationToken);
    }
    
    public async Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var response = await hybridCache.GetOrCreateAsync($"GitHub_Nightly",
            async token => await GetReleaseResponse(token),
            tags: ["release"],
            cancellationToken: cancellationToken);
        if (response == null)
            return null;
        return new LatestApplicationUpdate
        {
            Version = response.Name,
            Date = response.PublishedAt,
            Mandatory = !response.IsPrerelease,
            DownloadUrl = response.HtmlUrl,
            UpdateLog = response.Body,
            Size = -1,
        };
    }
}