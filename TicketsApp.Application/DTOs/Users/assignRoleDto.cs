using System;

namespace TicketsApp.Application.DTOs.Users;

public class assignRoleDto
{
    public int UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
