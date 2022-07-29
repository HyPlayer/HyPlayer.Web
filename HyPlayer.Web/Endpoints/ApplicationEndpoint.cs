using FluentValidation;
using HyPlayer.Web.Filters;
using HyPlayer.Web.Infrastructure;
using HyPlayer.Web.Infrastructure.Interfaces;
using HyPlayer.Web.Infrastructure.Models.DbModels;
using HyPlayer.Web.Infrastructure.Models.Packages.RequestPackages;
using HyPlayer.Web.Infrastructure.Models.Packages.ResponsePackages;
using HyPlayer.Web.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.Endpoints;

public class ApplicationEndpoint : IEndpoint
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IValidator<ApplicationCreateRequest>, ApplicationCreateValidation>();
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapPost("/application", CreateApplication)
            .AddRouteHandlerFilter<ValidationFilter<ApplicationCreateRequest>>()
            .WithSummary("Apply for insider test");
        app.MapGet("/applications", GetApplications)
            .AddRouteHandlerFilter<AuthenticationFilter>()
            .WithSummary("Get All Applications");
    }


    private static async Task<IResult> GetApplications(
        [FromQuery(Name = "channel")] ChannelType channelType,
        IRepository<User, Guid> repository,
        CancellationToken cancellationToken)
    {
        return Results.Json((await repository.GetQueryableEntitiesAsync(cancellationToken))
            .Where(t => t.ChannelType.HasFlag(channelType)).ToList());
    }

    private static async Task<IResult> CreateApplication(
        ApplicationCreateRequest request,
        IRepository<User, Guid> repository,
        ILogger<ApplicationEndpoint> logger,
        IEnumerable<IAppDistributor> appDistributors,
        CancellationToken cancellationToken)
    {
        // Check is already added
        if (await (await repository.GetQueryableEntitiesAsync(cancellationToken))
                .Where(t => t.Email == request.Email && t.ChannelType == request.ChannelType)
                .CountAsync(cancellationToken: cancellationToken) != 0)
        {
            return Results.UnprocessableEntity(new SimpleResponse("User already exist"));
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName!,
            Email = request.Email!,
            ChannelType = request.ChannelType,
            Contact = request.Contact!
        };
        logger.LogInformation("User {UserName} ({Email}) try to applicant for {ChannelType}", newUser.UserName,
            newUser.Email, newUser.ChannelType);


        var distributionAddResult = await (appDistributors
            .FirstOrDefault(x => x.BindingChannels.Contains(newUser.ChannelType))?
            .AddDistributionMemberAsync(newUser, cancellationToken) ?? Task.FromResult(false));

        if (!distributionAddResult)
        {
            return Results.BadRequest(new SimpleResponse("Failed to add to distribution"));
        }

        var result = await repository.CreateAsync(newUser, cancellationToken);
        return result
            ? Results.Created($"/user/{newUser.Id}", newUser)
            : Results.BadRequest(new SimpleResponse("User Not Created"));
    }
}