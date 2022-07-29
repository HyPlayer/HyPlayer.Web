using System.Text.Json.Serialization;
using HyPlayer.Web.Infrastructure.Models.DbModels;

namespace HyPlayer.Web.Infrastructure.Models.Packages.RequestPackages;

public class ApplicationCreateRequest
{
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }
    
    [JsonPropertyName("channel")]
    public int ChannelInt { get; set; }

    [JsonIgnore]
    public ChannelType ChannelType => (ChannelType)ChannelInt;
}