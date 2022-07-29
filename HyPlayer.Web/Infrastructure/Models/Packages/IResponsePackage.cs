using System.Text.Json.Serialization;

namespace HyPlayer.Web.Infrastructure.Models.Packages;

public interface IResponsePackage
{
    [JsonPropertyName("msg")]
    public string Message { get; set; }
}