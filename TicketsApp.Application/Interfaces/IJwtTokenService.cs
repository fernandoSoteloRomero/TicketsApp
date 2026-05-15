using System;

namespace TicketsApp.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(int userId, string email, IList<string> roles);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
}
