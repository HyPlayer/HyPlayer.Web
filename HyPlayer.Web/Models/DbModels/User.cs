using System.ComponentModel.DataAnnotations;

namespace HyPlayer.Web.Models.DbModels;

public class User
{
    public Guid Id { get; set; }

    [MaxLength(60)]
    public required string UserName { get; set; }

    [EmailAddress] 
    [MaxLength(60)]
    public required string Email { get; set; }
    
    [EmailAddress]
    [MaxLength(60)]
    public required string Contact { get; set; }
    
    public ChannelType ChannelType { get; set; }
    
    public bool Subscribe { get; set; }
}

[Flags]
public enum ChannelType
{
    StoreRelease,
    StoreBeta,
    AppCenterCanary,
    AppCenterRelease,
    GithubNightly,
    Canary,
    Release,
    Dogfood,
}

