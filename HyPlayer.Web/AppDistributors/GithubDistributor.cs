using System.Net.Mail;
using System.Text.Json.Serialization;
using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models;
using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.AppDistributors;

public class GithubDistributor : IAppDistributor
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateProvider _emailTemplateProvider;
    private readonly string _org;
    private readonly string _proj;
    private readonly string _tag;

    public GithubDistributor(IConfiguration configuration, IEmailService emailService,
        IEmailTemplateProvider emailTemplateProvider)
    {
        _emailService = emailService;
        _emailTemplateProvider = emailTemplateProvider;
        _org = configuration.GetValue<string>("Distributors:Github:OrganizationName")!;
        _proj = configuration.GetValue<string>("Distributors:Github:ProjectName")!;
        _tag = configuration.GetValue<string>("Distributors:Github:TagName")!;
    }

    private class GithubReleaseResponse
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("html_url")] public required string HtmlUrl { get; set; }

        [JsonPropertyName("prerelease")] public required bool IsPrerelease { get; set; }

        [JsonPropertyName("published_at")] public required DateTime PublishedAt { get; set; }

        [JsonPropertyName("body")] public required string Body { get; set; }
    }

    public List<ChannelType> BindingChannels => new() { ChannelType.GithubNightly };

    public async Task<bool> AddDistributionMemberAsync(User user, CancellationToken cancellationToken = default)
    {
        var template = await _emailTemplateProvider.GetTemplateAsync("GithubNightly", cancellationToken);
        return await _emailService.SendMailToAsync(user.Email, "[HyPlayer] 内测申请成功",
            template.Replace("{USERNAME}", user.UserName), cancellationToken);
    }

    public async Task<LatestApplicationUpdate?> GetLatestUpdateAsync(ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetFromJsonAsync<GithubReleaseResponse?>(
            $"https://api.github.com/repos/{_org}/{_proj}/releases/tags/{_tag}", cancellationToken: cancellationToken);
        if (response == null) return null;
        return new LatestApplicationUpdate
        {
            Version = response.Name,
            Date = response.PublishedAt,
            Mandatory = !response.IsPrerelease,
            ViewUrl = response.HtmlUrl,
            UpdateLog = response.Body
        };
    }
}