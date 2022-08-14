namespace HyPlayer.Web.Interfaces;

public interface IEmailService
{
    public Task<bool> SendMailToAsync(string to, string? subject = null, string? body = null, List<string>? bcc = null,
        CancellationToken cancellationToken = default);
}