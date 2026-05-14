using System;

namespace TicketsApp.Application.DTOs.Deppartments;

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
