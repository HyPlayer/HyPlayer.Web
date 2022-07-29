namespace HyPlayer.Web.Infrastructure.Interfaces;

public interface IEmailTemplateProvider
{
    public Task<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default);
}