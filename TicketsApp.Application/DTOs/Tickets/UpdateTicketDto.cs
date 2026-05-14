using System;

namespace TicketsApp.Application.DTOs.Tickets;

public class UpdateTicketDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public int? AssignedToUserId { get; set; }
    public int? AssignedToDepartmentId { get; set; }
}
