using System;

namespace TicketsApp.Domain.Entities;

public class AuditLog
{
    public int AuditLogId { get; set; }
    public int PerformedByUserId { get; set; }
    public string Action { get; set; } = string.Empty; // USER_CREATED, ROLE_CHANGED, etc
    public string ResourceType { get; set; } = string.Empty; // User, Department, Ticket, etc
    public string ResourceId { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

// Relaciones
    public User? PerformedByUser { get; set; }
}
