namespace HyPlayer.Web.Infrastructure.Models.Packages.ResponsePackages;

public class SimpleResponse : IResponsePackage
{
    public string Message { get; set; }

    public SimpleResponse(string message)
    {
        Message = message;
    }
}