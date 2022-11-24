using FluentValidation;
using HyPlayer.Web.Filters;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;
using HyPlayer.Web.Models.Packages;
using HyPlayer.Web.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.Endpoints;

public class UserEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IValidator<UserCreateRequest>, ApplicationCreateValidation>();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapPost("/user", CreateUser)
            .AddEndpointFilter<ValidationFilter<UserCreateRequest>>();
        app.MapGet("/users", GetUsers)
            .AddEndpointFilter<AuthenticationFilter>();
        app.MapGet("/user/{userId:guid}", GetUser);
        app.MapGet("/user/email/{email}", GetUserByEmail);
        app.MapGet("/user/delete/{id:guid}", UserUnsubscribe);
    }

    private async Task<IResult> GetUser(Guid userId,
        IRepository<User, Guid> repository,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(userId, cancellationToken);
        if (user != null)
        {
            if (user.IsBanned)
            {
                Results.BadRequest("该用户已被封禁");
            }
            else
            {
                return Results.Ok(user);
            }
        }
        return Results.NotFound("用户不存在");
    }

    private async Task<IResult> GetUserByEmail(string email,
        IRepository<User, Guid> repository,
        CancellationToken cancellationToken)
    {
        var user = (await repository.GetQueryableEntitiesAsync(cancellationToken))
            .FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            if (user.IsBanned)
            {
                Results.BadRequest("该用户已被封禁");
            }
            else
            {
                return Results.Ok(user);
            }
        }
        return Results.NotFound("用户不存在");
    }


    private static async Task<IResult> UserUnsubscribe([FromRoute] Guid id,
        IRepository<User, Guid> repository,
        CancellationToken cancellationToken)
    {
        if (await repository.GetByIdAsync(id, cancellationToken) is { } user)
        {
            user.Subscribe = false;
            await repository.UpdateAsync(user, cancellationToken);
            return Results.Ok("成功退出通道");
        }
        else
        {
            return Results.NotFound("用户不存在");
        }
    }


    private static async Task<IResult> GetUsers(
        [FromQuery(Name = "channel")] ChannelType channelType,
        IRepository<User, Guid> repository,
        CancellationToken cancellationToken)
    {
        return Results.Json((await repository.GetQueryableEntitiesAsync(cancellationToken))
            .Where(t => t.ChannelType.HasFlag(channelType)).ToList());
    }

    private static async Task<IResult> CreateUser(
        UserCreateRequest request,
        IRepository<User, Guid> repository,
        ILogger<UserEndpoint> logger,
        IEnumerable<IAppDistributor> appDistributors,
        IEmailService emailService,
        IEmailTemplateProvider templateProvider,
        CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName!,
            Email = request.Email!,
            ChannelType = request.ChannelType,
            Contact = request.Contact!,
            Subscribe = request.SubscribeUpdates,
            IsBanned = false
        };


        var distributionAddResult = /*await (appDistributors
            .FirstOrDefault(x => x.BindingChannels.Contains(newUser.ChannelType))?
            .AddDistributionMemberAsync(newUser, cancellationToken) ?? Task.FromResult(false));*/ true;

        if (!distributionAddResult)
        {
            return Results.BadRequest("添加到分发组失败");
        }

        bool repositoryResult;
        var dbUser =
            (await repository.GetQueryableEntitiesAsync(cancellationToken)).FirstOrDefault(
                t => t.Email == request.Email);
        if (dbUser != null)
        {
            if (dbUser.IsBanned == false)
            {
                dbUser.Contact = newUser.Contact;
                dbUser.UserName = newUser.UserName;
                dbUser.ChannelType = newUser.ChannelType;
                dbUser.Subscribe = newUser.Subscribe;
                newUser.Id = dbUser.Id;
                dbUser.IsBanned = false;
                repositoryResult = await repository.UpdateAsync(dbUser, cancellationToken);
            }
            else return Results.BadRequest("该用户已被封禁");
        }
        else
        {
            repositoryResult = await repository.CreateAsync(newUser, cancellationToken);
        }
        

        return repositoryResult
            ? Results.Created($"/user/{newUser.Id}", newUser)
            : Results.BadRequest("用户未创建");
    }
}