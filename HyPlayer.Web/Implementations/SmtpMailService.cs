using System.Net;
using System.Net.Mail;
using HyPlayer.Web.Infrastructure.Interfaces;

namespace HyPlayer.Web.Implementations;

public class SmtpMailService : IEmailService
{
    private readonly string? _username;
    private readonly string? _password;
    private readonly string? _host;
    private readonly int _port;
    private readonly string? _from;
    private readonly bool _useSsl;
    private readonly SmtpClient _smtpClient;

    public SmtpMailService(IConfiguration configuration)
    {
        _username = configuration.GetValue<string>("Smtp:UserName");
        _password = configuration.GetValue<string>("Smtp:Password");
        _host = configuration.GetValue<string>("Smtp:Host");
        _port = configuration.GetValue<int>("Smtp:Port");
        _from = configuration.GetValue<string>("Smtp:From");
        _useSsl = configuration.GetValue<bool>("Smtp:UseSsl");
        _smtpClient = new SmtpClient(_host,_port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = _useSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
    }

    public async Task<bool> SendMailToAsync(string to, string subject, string body,
        CancellationToken cancellationToken = default)
    {
        await _smtpClient.SendMailAsync(_from!, to, subject, body, cancellationToken);
        return true;
    }
}