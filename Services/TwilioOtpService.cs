using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace template_backend.Services;

public class TwilioOtpService
{
    private readonly IConfiguration _config;

    public TwilioOtpService(IConfiguration config)
    {
        _config = config;

        TwilioClient.Init(
            _config["Twilio:AccountSid"],
            _config["Twilio:AuthToken"]
        );
    }

    public async Task SendSmsOtpAsync(string phoneNumber, string otp)
    {
        await MessageResource.CreateAsync(
            to: phoneNumber,
            from: _config["Twilio:FromNumber"],
            body: $"Your OTP is: {otp}"
        );
    }
}
