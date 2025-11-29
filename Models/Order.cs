namespace template_backend.Models;
using System.Text.Json.Serialization;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Pending";

    public decimal TotalAmount { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    // Admin Delivery
    public string? DeliveryOtp { get; set; } // stores 6-digit OTP during delivery

    public DateTime? DeliveryOtpExpiresAt { get; set; }
}
