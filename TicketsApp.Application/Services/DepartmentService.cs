using System;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Application.DTOs.Deppartments;
using TicketsApp.Application.Interfaces;
using TicketsApp.Infrastructure.Data;

namespace TicketsApp.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;

    public DepartmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _context.Departments
            .Where(d => d.IsActive)
            .ToListAsync();

        return departments.Adapt<List<DepartmentDto>>();
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

        if (department == null)
            throw new InvalidOperationException("Departamento no encontrado");

        return department.Adapt<DepartmentDto>();
    }
}