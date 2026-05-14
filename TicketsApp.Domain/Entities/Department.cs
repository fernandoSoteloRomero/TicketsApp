using System;

namespace TicketsApp.Domain.Entities;

public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty; // IT, RH, Finanzas, etc
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

// Relaciones
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
