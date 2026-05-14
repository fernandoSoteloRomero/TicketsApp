using System;

namespace TicketsApp.Domain.Entities;

public class TicketAttachment
{
    public int AttachmentId { get; set; }
    public int TicketId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty; // Ruta o URL del archivo
    public long FileSize { get; set; } // En bytes
    public int UploadedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

// Relaciones
    public Ticket? Ticket { get; set; }
    public User? UploadedByUser { get; set; }
}
