using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Admin
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
    [MinLength(12)]
    public string Password { get; set; } = null!;

    public int PhoneNumber { get; set; }

    public string? Role { get; set; } = "Admin";
}
