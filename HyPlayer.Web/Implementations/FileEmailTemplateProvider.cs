using HyPlayer.Web.Interfaces;

namespace HyPlayer.Web.Implementations;

public class FileEmailTemplateProvider : IEmailTemplateProvider
{
    public async Task<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        return await File.ReadAllTextAsync($"data/Templates/Mail/{templateName}.txt", cancellationToken);
    }
}