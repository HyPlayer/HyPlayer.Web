using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HyPlayer.Web.Models.DbModels;

namespace HyPlayer.Web.Models.Packages;

public class ReleaseCreateRequest
{
    [JsonPropertyName("channels")]
    public ChannelType[] Channels { get; set; } = [];
    
    [JsonPropertyName("version")]
    [MaxLength(30)] public required string Version { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime ReleaseDate { get; set; } = DateTime.Now;
    
    [JsonPropertyName("note")]
    public required string ReleaseNotes { get; set; }
    
    [JsonPropertyName("url")]
    public required string DownloadUrl { get; set; }
    
    [JsonPropertyName("authKey")]
    public required string AuthKey { get; set; }
}