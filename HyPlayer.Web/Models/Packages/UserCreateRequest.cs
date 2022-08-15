using System.Text.Json.Serialization;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Models.Packages;

public class UserCreateRequest
{
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }
    
    [JsonPropertyName("channel")]
    public int ChannelInt { get; set; }
    
    [JsonPropertyName("subscribe")]
    public bool SubscribeUpdates { get; set; }

    [JsonIgnore]
    public ChannelType ChannelType => (ChannelType)ChannelInt;
}