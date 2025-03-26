using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Models.DbModels;
using HyPlayer.Web.Models.Packages;
using Microsoft.AspNetCore.Mvc;

namespace HyPlayer.Web.Endpoints;

public class ReleaseEndpoint : IEndpoint
{

    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void ConfigureEndpoint(WebApplication app)
    {
        app.MapGet("/releases", GetReleases);
        app.MapGet("/release/{releaseId:guid}", GetRelease);
        app.MapPut("/releases", AddRelease);
    }

    private async Task<IResult> GetReleases(IRepository<Release, Guid> repository,
        CancellationToken cancellationToken, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var releases = (await repository.GetQueryableEntitiesAsync(cancellationToken))
            .OrderByDescending(t => t.ReleaseDate).Skip(limit).Take(10);
        return Results.Ok(releases);
    }
    
    private async Task<IResult> GetRelease(Guid releaseId,
        IRepository<Release, Guid> repository,
        CancellationToken cancellationToken)
    {
        var release = await repository.GetByIdAsync(releaseId, cancellationToken);
        return release != null ? Results.Ok(release) : Results.NotFound("Release Not Found");
    }

    private async Task<IResult> AddRelease(IRepository<Release, Guid> repository,
        [FromServices] IConfiguration configuration,
        [FromBody] ReleaseCreateRequest request)
    {
        if (configuration.GetValue<string>("PipelineAuthKey") != request.AuthKey)
            return Results.Problem("授权密钥有误", statusCode: 403);
        var release = new Release
        {
            Id = Guid.CreateVersion7(),
            Channels = request.Channels,
            Version = request.Version,
            ReleaseDate = request.ReleaseDate,
            ReleaseNotes = request.ReleaseNotes,
            DownloadUrl = request.DownloadUrl,
        };
        await repository.CreateAsync(release);
        return Results.Created($"/release/{release.Id}", release);
    }
    
}