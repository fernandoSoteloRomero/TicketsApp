using System;
using TicketsApp.Application.DTOs.Users;

namespace TicketsApp.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<bool> UserExistsAsync(string email);
    Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
}
