﻿using System.ComponentModel.DataAnnotations;

namespace UserProvider.Data.Entities;

public class UserProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? ImageUrl { get; set; }

    public UserEntity User { get; set; } = null!;
}
