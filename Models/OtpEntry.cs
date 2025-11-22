namespace template_backend.Models;

public class OtpEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Identifier { get; set; } = default!;
    public string Otp { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
