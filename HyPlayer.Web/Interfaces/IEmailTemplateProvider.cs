namespace HyPlayer.Web.Interfaces;

public interface IEmailTemplateProvider
{
    public Task<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default);
}