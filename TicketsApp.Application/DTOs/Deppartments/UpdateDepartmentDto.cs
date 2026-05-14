using System;

namespace TicketsApp.Application.DTOs.Deppartments;

public class UpdateDepartmentDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
