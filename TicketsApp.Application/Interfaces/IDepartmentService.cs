using System;
using TicketsApp.Application.DTOs.Deppartments;

namespace TicketsApp.Application.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);
}
