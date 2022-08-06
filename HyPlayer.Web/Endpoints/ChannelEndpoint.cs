using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Endpoints;

public class ChannelEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Nothing
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapGet("/channel/{channel}/latest", GetDistributorChannelUpdate);
        app.MapGet("/channel/{channel}/latest/{userId:guid}", GetDistributorChannelUserUpdate);
        app.MapGet("/channel/{channel}/broadcastUpdate/{authKey}", BroadcastUpdate);
    }

    private async Task<IResult> BroadcastUpdate(ChannelType channel,
        IUpdateBroadcaster broadcaster,
        IRepository<User, Guid> repository,
        IConfiguration configuration,
        string authKey)
    {
        if (configuration.GetValue<string>("PipelineAuthKey") != authKey)
            return Results.Problem("授权密钥有误", statusCode: 403);
        _ = broadcaster.BroadcastAsync(channel,
            (await repository.GetQueryableEntitiesAsync()).Where(t => t.ChannelType == channel && t.Subscribe)
            .ToList());
        return Results.Ok();
    }

    private async Task<IResult> GetDistributorChannelUserUpdate(CancellationToken cancellationToken,
        ChannelType channel,
        Guid userId,
        IEnumerable<IAppDistributor> appDistributors,
        IRepository<User, Guid> repository)
    {
        var user = await repository.GetByIdAsync(userId, cancellationToken);
        if (user?.ChannelType != channel) return Results.Problem("用户不存在或未订阅此通道", statusCode: 403);
        var appDistributor = appDistributors.FirstOrDefault(t => t.BindingChannels.Contains(channel));
        if (appDistributor == null)
            return Results.NotFound("通道未知");
        var update = (await appDistributor.GetLatestUpdateAsync(channel, cancellationToken));
        return update == null ? Results.NotFound("获取更新失败") : Results.Ok(update);
    }

    private async Task<IResult> GetDistributorChannelUpdate(ChannelType channel,
        IEnumerable<IAppDistributor> appDistributors,
        CancellationToken cancellationToken)
    {
        var appDistributor = appDistributors.FirstOrDefault(t => t.BindingChannels.Contains(channel));
        if (appDistributor == null)
            return Results.NotFound("通道不存在");
        var update = (await appDistributor.GetLatestUpdateAsync(channel, cancellationToken));
        if (update == null) return Results.NotFound("获取更新失败");
        update.DownloadUrl = $"https://hyplayer.kengwang.com.cn/#/channel/{(int)channel}/latest";
        return Results.Ok(update);
    }
}