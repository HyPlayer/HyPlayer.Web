namespace HyPlayer.Web.Infrastructure.Interfaces;

public interface IEmailService
{
    public Task<bool> SendMailToAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}