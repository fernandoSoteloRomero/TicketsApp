using System;

namespace TicketsApp.Domain.Entities;

public class TicketHistory
{
    public int HistoryId { get; set; }
    public int TicketId { get; set; }
    public int ChangedByUserId { get; set; }
    public string FieldChanged { get; set; } = string.Empty; // status, assignedToUserId, etc
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public string ChangeType { get; set; } = string.Empty; // CREATED, UPDATED, ASSIGNED, REASSIGNED, COMMENTED
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

// Relaciones
    public Ticket? Ticket { get; set; }
    public User? ChangedByUser { get; set; }
}
