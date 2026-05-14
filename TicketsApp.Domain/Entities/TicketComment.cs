using System;

namespace TicketsApp.Domain.Entities;

public class TicketComment
{
    public int CommentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

// Relaciones
    public Ticket? Ticket { get; set; }
    public User? User { get; set; }
}
