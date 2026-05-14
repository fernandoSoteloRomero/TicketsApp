using System;

namespace TicketsApp.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty; // Empleado, Agente, GerenteDpto, Admin
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

// Relaciones
    public ICollection<User> Users { get; set; } = new List<User>();
}
