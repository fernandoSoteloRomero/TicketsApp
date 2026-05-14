using System;

namespace TicketsApp.Application.DTOs.Tickets;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "MEDIA";
    public int AssignedToDepartmentId { get; set; }
    public int? AssignedToUserId { get; set; }
}
