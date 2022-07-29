using System.ComponentModel.DataAnnotations;

namespace HyPlayer.Web.Infrastructure.Models.DbModels;

public class User
{
    public Guid Id { get; set; }

    public required string UserName { get; set; }

    [EmailAddress] public required string Email { get; set; }
    [EmailAddress] public required string Contact { get; set; }
    
    public ChannelType ChannelType { get; set; }
}

[Flags]
public enum ChannelType
{
    StoreRelease,
    StoreBeta,
    AppCenterCanary,
    AppCenterRelease,
    GithubNightly
}