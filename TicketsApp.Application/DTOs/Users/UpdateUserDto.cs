using System;

namespace TicketsApp.Application.DTOs.Users;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? DepartmentId { get; set; }
    public string? Role { get; set; }
}
