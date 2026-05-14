using System;

namespace TicketsApp.Application.DTOs.Comments;

public class CreateCommentDto
{
    public int TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
}
