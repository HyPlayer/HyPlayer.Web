namespace HyPlayer.Web.Infrastructure.Models.Packages.ResponsePackages;

public class ValidationFailedResponse : IResponsePackage
{
    public string Message { get; set; } = "Validation Failed";
    public IEnumerable<string>? Errors { get; set; }
}