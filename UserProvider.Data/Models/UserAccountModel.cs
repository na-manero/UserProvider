using System.ComponentModel.DataAnnotations;

namespace UserProvider.Data.Models;

public class UserAccountModel
{
    public string? UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
}
