using System;
using TicketsApp.Domain.Enums;

namespace TicketsApp.Application.DTOs.Auth;

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string? Role { get; set; }
}
