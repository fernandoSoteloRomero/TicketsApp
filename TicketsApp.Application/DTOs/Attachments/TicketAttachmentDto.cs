using System;

namespace TicketsApp.Application.DTOs.Attachments;

public class TicketAttachmentDto
{
    public int AttachmentId { get; set; }
    public int TicketId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int UploadedByUserId { get; set; }
    public string? UploadedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}
