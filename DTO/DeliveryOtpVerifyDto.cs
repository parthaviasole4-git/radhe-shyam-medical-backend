namespace template_backend.DTO
{
    public class DeliveryOtpVerifyDto
    {
        public Guid OrderId { get; set; }
        public string Otp { get; set; }
    }
}
