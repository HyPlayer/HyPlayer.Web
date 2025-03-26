using System.ComponentModel.DataAnnotations;

namespace HyPlayer.Web.Models.DbModels;

public class Release
{
    public Guid Id { get; init; }
    public ChannelType[] Channels { get; set; } = [];
    [MaxLength(30)] public required string Version { get; set; }
    public DateTime ReleaseDate { get; set; }
    public required string ReleaseNotes { get; set; }
    public required string DownloadUrl { get; set; }
    public bool Mandatory { get; set; } = false;
}