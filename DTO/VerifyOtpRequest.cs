namespace template_backend.DTO
{
    public class VerifyOtpRequest
    {
        public required string Identifier { get; set; }
        public required string Code { get; set; }
    }
}
