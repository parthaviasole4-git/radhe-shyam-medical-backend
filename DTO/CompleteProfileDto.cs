namespace template_backend.Models.DTOs;

public class CompleteProfileDto
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address Address { get; set; }
}

