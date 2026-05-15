using System;
using TicketsApp.Application.DTOs.Auth;

namespace TicketsApp.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto request);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(int userId);

    Task<RegisterResponseDto> RegisterAsync(RegisterDto request);
}
