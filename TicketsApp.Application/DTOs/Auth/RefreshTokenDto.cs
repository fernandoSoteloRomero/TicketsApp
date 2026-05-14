using System;

namespace TicketsApp.Application.DTOs.Auth;

public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}
