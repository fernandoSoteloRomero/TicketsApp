using System;
using TicketsApp.Application.DTOs.Attachments;
using TicketsApp.Application.DTOs.Comments;

namespace TicketsApp.Application.DTOs.Tickets;

public class TicketDto
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public int AssignedToDepartmentId { get; set; }
    public string? AssignedToDepartmentName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public ICollection<TicketCommentDto> Comments { get; set; } = new List<TicketCommentDto>();
    public ICollection<TicketAttachmentDto> Attachments { get; set; } = new List<TicketAttachmentDto>();
}
