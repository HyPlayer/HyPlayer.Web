using System.Globalization;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Implementations;

public class EmailUpdateBroadcaster : IUpdateBroadcaster
{
    private readonly IEmailTemplateProvider _emailTemplateProvider;
    private readonly IEmailService _emailService;
    private readonly IEnumerable<IAppDistributor> _appDistributors;

    public EmailUpdateBroadcaster(IEmailTemplateProvider emailTemplateProvider, IEmailService emailService,
        IEnumerable<IAppDistributor> appDistributors)
    {
        _emailTemplateProvider = emailTemplateProvider;
        _emailService = emailService;
        _appDistributors = appDistributors;
    }

    public Task BroadcastAsync(ChannelType type, List<User> users)
    {
        return Task.Run(async () =>
        {
            var appDistributor = _appDistributors.FirstOrDefault(t => t.BindingChannels.Contains(type));
            if (appDistributor == null) throw new Exception("通道不存在");
            var update = await appDistributor.GetLatestUpdateAsync(type);
            if (update == null) throw new Exception("更新获取失败");
            foreach (var user in users)
            {
                var template = await _emailTemplateProvider.GetTemplateAsync("ChannelUpdateNotice");
                var msg = template
                    .Replace("{USERNAME}", user.UserName)
                    .Replace("{VERSION}", update.Version)
                    .Replace("{TIME}", update.Date.ToString(CultureInfo.CurrentCulture))
                    .Replace("{UPDATELOG}", update.UpdateLog)
                    .Replace("{CHANNELID}", ((int)type).ToString())
                    .Replace("{USERID}", user.Id.ToString());
                await _emailService.SendMailToAsync(user.Contact, "[HyPlayer] 您所订阅的更新通道已经推出新版本", msg);
                await Task.Delay(5000);
            }
        });
    }
}