using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Application.DTOs.Users;
using TicketsApp.Application.Interfaces;
using TicketsApp.Domain.Entities;
using TicketsApp.Infrastructure.Data;

namespace TicketsApp.Application.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

    public UserService(
        ApplicationDbContext context,
        UserManager<User> userManager,
        IValidator<ChangePasswordDto> changePasswordValidator)
    {
        _context = context;
        _userManager = userManager;
        _changePasswordValidator = changePasswordValidator;
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        var userDto = user.Adapt<UserDto>();
        userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

        return userDto;
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        var userDto = user.Adapt<UserDto>();
        userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

        return userDto;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userManager.Users.AnyAsync(u => u.Email == email);
    }

    public async Task UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        var userRoles = await _userManager.GetRolesAsync(user);

        if (!string.IsNullOrEmpty(updateUserDto.FirstName))
            user.UserName = updateUserDto.FirstName;

        if (updateUserDto.DepartmentId.HasValue)
            user.DepartmentId = updateUserDto.DepartmentId;

        if (!string.IsNullOrWhiteSpace(updateUserDto.Role) && !userRoles.Contains(updateUserDto.Role))
        {
            var roleInDb = await _context.Roles.FirstOrDefaultAsync(r => r.Name == updateUserDto.Role) ??
                           throw new InvalidOperationException($"El rol: {updateUserDto.Role} no existe");
            await _userManager.AddToRoleAsync(user, roleInDb.Name);
        }

        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("No se pudo actualizar el usuario");
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        // Validar
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        var result = await _userManager.ChangePasswordAsync(
            user,
            changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword
        );

        if (!result.Succeeded)
            throw new InvalidOperationException("No se pudo cambiar la contraseña");
    }
}
