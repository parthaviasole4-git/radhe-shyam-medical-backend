
public class Address
{
    public string House { get; set; }
    public string Area { get; set; }
    public string City { get; set; }
    public string Pincode { get; set; }
    public string State { get; set; }
}
public class User
{
    public Guid Id { get; set; }

    public string Identifier { get; set; } // email or phone used for login

    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public Address? Address { get; set; }

    public bool IsAdmin { get; set; } = false;
    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

