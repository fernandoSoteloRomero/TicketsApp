using System;

namespace TicketsApp.Application.DTOs.Attachments;

public class CreateAttachmentDto
{
    public int TicketId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
}
