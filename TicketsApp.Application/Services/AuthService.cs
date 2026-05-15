using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Application.DTOs.Auth;
using TicketsApp.Application.Interfaces;
using TicketsApp.Domain.Entities;
using TicketsApp.Infrastructure.Data;

namespace TicketsApp.Application.Services;

public class AuthService : IAuthService
{
  private readonly ApplicationDbContext _context;
  private readonly IJwtTokenService _jwtTokenService;
  private readonly UserManager<User> _userManager;

  public AuthService(
    ApplicationDbContext context,
    IJwtTokenService jwtTokenService,
    UserManager<User> userManager)
  {
    _context = context;
    _jwtTokenService = jwtTokenService;
    _userManager = userManager;
  }

  public async Task<LoginResponseDto> LoginAsync(LoginDto request)
  {
    //! Buscar usuario por email
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
      throw new UnauthorizedAccessException("Email o contraseña inválidos");

    if (!user.IsActive)
      throw new UnauthorizedAccessException("Usuario inactivo");

    //! Obtener roles del usuario
    var roles = await _userManager.GetRolesAsync(user);

    //! Generar tokens
    var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
    var refreshToken = _jwtTokenService.GenerateRefreshToken();

    //! Guardar refresh token en BD
    var refreshTokenEntity = new RefreshToken
    {
      UserId = user.Id,
      Token = refreshToken,
      ExpiryDate = DateTime.UtcNow.AddDays(7),
      IsRevoked = false
    };

    _context.Set<RefreshToken>().Add(refreshTokenEntity);
    await _context.SaveChangesAsync();

    return new LoginResponseDto
    {
      UserId = user.Id,
      Email = user.Email!,
      FirstName = user.FirstName,
      LastName = user.LastName,
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      ExpiresIn = 15 * 60
    };
  }

  public async Task LogoutAsync(int userId)
  {
    //! Marcar todos los refresh tokens del usuario como revocados
    var tokens = await _context.Set<RefreshToken>()
      .Where(rt => rt.UserId == userId && !rt.IsRevoked)
      .ToListAsync();

    foreach (var token in tokens)
    {
      token.IsRevoked = true;
    }

    await _context.SaveChangesAsync();
  }

  public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
  {
    //! Buscar el refresh token
    var storedToken = await _context.Set<RefreshToken>()
      .Include(rt => rt.User)
      .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

    if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
      throw new UnauthorizedAccessException("Refresh token inválido o expirado");

    var user = storedToken.User ?? throw new UnauthorizedAccessException("Refresh token inválido o expirado");
    var roles = await _userManager.GetRolesAsync(user);

    //! 🔒 PASO 1: Revocar el token anterior
    storedToken.IsRevoked = true;
    _context.Set<RefreshToken>().Update(storedToken);

    //! ✨ PASO 2: Generar NUEVO refresh token
    var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
    var newRefreshTokenEntity = new RefreshToken
    {
      UserId = user.Id,
      Token = newRefreshToken,
      ExpiryDate = DateTime.UtcNow.AddDays(7),
      IsRevoked = false,
      CreatedAt = DateTime.UtcNow
    };
    _context.Set<RefreshToken>().Add(newRefreshTokenEntity);
    await _context.SaveChangesAsync();

    //! PASO 3: Generar nuevo access token
    var newAccessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);

    return new TokenResponseDto
    {
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken, //! ← NUEVO token, no el anterior
      ExpiresIn = 15 * 60
    };
  }
}
