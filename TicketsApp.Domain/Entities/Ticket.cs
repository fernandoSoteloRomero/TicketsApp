using System;

namespace TicketsApp.Domain.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty; // TKT-0001, TKT-0002, etc
    public int CreatedByUserId { get; set; }
    public int? AssignedToUserId { get; set; } // Nullable si es asignación al departamento
    public int AssignedToDepartmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "ABIERTO"; // ABIERTO, EN_PROGRESO, PENDIENTE_INFO, RESUELTO, CERRADO
    public string Priority { get; set; } = "MEDIA"; // BAJA, MEDIA, ALTA, URGENTE
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

// Relaciones
    public User? CreatedByUser { get; set; }
    public User? AssignedToUser { get; set; }
    public Department? AssignedToDepartment { get; set; }
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
    public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
    public ICollection<TicketHistory> History { get; set; } = new List<TicketHistory>();
}
