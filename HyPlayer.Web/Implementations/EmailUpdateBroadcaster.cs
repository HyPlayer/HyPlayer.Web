using System.Globalization;
using System.Net.Mail;
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

    public async Task<bool> BroadcastAsync(ChannelType type, List<User> users)
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

            var mails = users.Select(user => user.Contact.Replace(" ", "")).ToList();
            foreach (var mail in mails)
            {
                try
                {
                    var a = new MailAddress(mail);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "邮箱 {Contact} 存在问题", mail);
                }
            }

            for (int i = 0; i < mails.Count; i += 100)
            {
                await _emailService.SendMailToAsync("hyplayer123@163.com", "[HyPlayer] 您所订阅的更新通道已经推出新版本", msg,
                    bcc: mails.GetRange(i, Math.Min(100, mails.Count - i)));
                await Task.Delay(5000);
            }


            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Broadcasting Update");
            return false;
        }
    }
}