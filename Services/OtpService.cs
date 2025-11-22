using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace template_backend.Services;

public class OtpService
{
    private readonly IConfiguration _config;
    public OtpService(IConfiguration config) => _config = config;

    public async Task SendOtpAsync(string email, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_config["Smtp:Email"]));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "Your Verification Code";

        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP is: {otp}"
        };

        using var client = new SmtpClient();

        // IMPORTANT FIX
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _config["Smtp:Email"],
            _config["Smtp:Password"]
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
