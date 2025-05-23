﻿using System.Globalization;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;
using Kvyk.Telegraph;
using Kvyk.Telegraph.Models;
using Telegram.Bot;

namespace HyPlayer.Web.Implementations;

public class TelegramBroadcaster(
    IConfiguration configuration,
    IEnumerable<IAppDistributor> appDistributors,
    ILogger<TelegramBroadcaster> logger)
    : IUpdateBroadcaster, IDisposable
{
    private readonly HttpClient _httpClient = new();
    private readonly TelegraphClient _telegraphClient = new(configuration.GetValue<string>("Telegraph:AccessToken"));
    private readonly TelegramBotClient _telegramBotClient = new(configuration.GetValue<string>("Telegram:BotToken")!);

    public async Task<bool> BroadcastAsync(ChannelType type, List<User> users)
    {
        try
        {
            var appDistributor = appDistributors.FirstOrDefault(t => t.BindingChannels.Contains(type));
            if (appDistributor == null) throw new Exception("通道不存在");
            var update = await appDistributor.GetLatestUpdateAsync(type);
            if (update == null) throw new Exception("更新获取失败");

            // Create a Telegraph
            var page = await _telegraphClient.CreatePage($"[版本更新] {update.Version}", new List<Node>()
            {
                Node.H3("HyPlayer 发布更新"),
                Node.P($"版本: {update.Version}"),
                Node.P($"更新日期: {update.Date.ToString(CultureInfo.InvariantCulture)} (UTC)"),
                Node.P($"更新通道: {type.ToString()}"),
                Node.H4("更新日志: "),
                Node.P(update.UpdateLog),
                Node.P(),
                Node.P(
                    new List<Node>()
                    {
                        Node.P("在此通道的用户可前往"),
                        Node.A("https://hyplayer.kengwang.com.cn/#/channel/latest"),
                        Node.P("获取更新")
                    }),
            });

            // Send Telegram Message
            await _telegramBotClient.SendTextMessageAsync("@hyplayer", page.Url);
            return true;
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "广播到 Telegram 发生错误");
            return false;
        }
    }

    public void Dispose() => _httpClient.Dispose();
}