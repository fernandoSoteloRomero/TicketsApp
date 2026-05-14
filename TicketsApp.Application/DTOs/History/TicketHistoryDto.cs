using System;

namespace TicketsApp.Application.DTOs.History;

public class TicketHistoryDto
{
    public int HistoryId { get; set; }
    public int TicketId { get; set; }
    public int ChangedByUserId { get; set; }
    public string ChangedByUserName { get; set; } = string.Empty;
    public string FieldChanged { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public string ChangeType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}