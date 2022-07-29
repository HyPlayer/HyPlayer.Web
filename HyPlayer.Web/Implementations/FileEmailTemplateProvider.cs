using HyPlayer.Web.Infrastructure.Interfaces;

namespace HyPlayer.Web.Implementations;

public class FileEmailTemplateProvider : IEmailTemplateProvider
{
    public async Task<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllTextAsync($"Templates/Mail/{templateName}.txt", cancellationToken);
    }
}