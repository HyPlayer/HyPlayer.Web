using HyPlayer.Web.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HyPlayer.Web.Implementations;

public class SmtpMailService : IEmailService , IDisposable
{
    private readonly string? _username;
    private readonly string? _password;
    private readonly string? _host;
    private readonly int _port;
    private readonly string? _from;
    private readonly bool _useSsl;
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<SmtpMailService> _logger;

    public SmtpMailService(IConfiguration configuration, ILogger<SmtpMailService> logger)
    {
        _logger = logger;
        _username = configuration.GetValue<string>("Smtp:UserName");
        _password = configuration.GetValue<string>("Smtp:Password");
        _host = configuration.GetValue<string>("Smtp:Host");
        _port = configuration.GetValue<int>("Smtp:Port");
        _from = configuration.GetValue<string>("Smtp:From");
        _useSsl = configuration.GetValue<bool>("Smtp:UseSsl");
        _smtpClient = new SmtpClient();
        _smtpClient.Connect(_host, _port, SecureSocketOptions.StartTls);
        _smtpClient.Authenticate(_username, _password);
    }

    public async Task<bool> SendMailToAsync(string to, string? subject, string? body, List<string>? bcc,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending mail to {To} with {BccCount} bcc", to, bcc?.Count);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("HyPlayer Team", _from!));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };
        if (bcc != null) message.Bcc.AddRange(bcc.Select(t => new MailboxAddress(t, t)));
        await _smtpClient.SendAsync(message, cancellationToken);
        return true;
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
    }
}