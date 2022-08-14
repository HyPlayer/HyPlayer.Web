using System.Globalization;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Implementations;

public class EmailUpdateBroadcaster : IUpdateBroadcaster
{
    private readonly IEmailTemplateProvider _emailTemplateProvider;
    private readonly IEmailService _emailService;
    private readonly IEnumerable<IAppDistributor> _appDistributors;
    private readonly ILogger<EmailUpdateBroadcaster> _logger;

    public EmailUpdateBroadcaster(IEmailTemplateProvider emailTemplateProvider, IEmailService emailService,
        IEnumerable<IAppDistributor> appDistributors, ILogger<EmailUpdateBroadcaster> logger)
    {
        _emailTemplateProvider = emailTemplateProvider;
        _emailService = emailService;
        _appDistributors = appDistributors;
        _logger = logger;
    }

    public Task BroadcastAsync(ChannelType type, List<User> users)
    {
        return Task.Run(async () =>
        {
            try
            {
                var appDistributor = _appDistributors.FirstOrDefault(t => t.BindingChannels.Contains(type));
                if (appDistributor == null) throw new Exception("通道不存在");
                var update = await appDistributor.GetLatestUpdateAsync(type);
                if (update == null) throw new Exception("更新获取失败");

                var template = await _emailTemplateProvider.GetTemplateAsync("ChannelUpdateNotice");
                var msg = template
                    .Replace("{VERSION}", update.Version)
                    .Replace("{TIME}", update.Date.ToString(CultureInfo.CurrentCulture))
                    .Replace("{UPDATELOG}", update.UpdateLog)
                    .Replace("{CHANNELID}", ((int)type).ToString());
                await _emailService.SendMailToAsync("hyplayer123@163.com", "[HyPlayer] 您所订阅的更新通道已经推出新版本", msg,
                    bcc: users.Select(t => t.Contact).ToList());
                await Task.Delay(5000);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Broadcasting Update");
            }
        });
    }
}