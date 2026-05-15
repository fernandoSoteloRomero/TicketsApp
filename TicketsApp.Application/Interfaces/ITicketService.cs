using System;
using TicketsApp.Application.DTOs.Tickets;

namespace TicketsApp.Application.Interfaces;

public interface ITicketService
{
    Task<TicketDto> CreateTicketAsync(
        CreateTicketDto createTicketDto, int createdByUserId);

    Task<TicketDto> GetTicketByIdAsync(int ticketId);

    Task<List<TicketListDto>> GetTicketsAsync(int? departmentId = null,
        string? status = null);

    Task UpdateTicketAsync(int ticketId, UpdateTicketDto updateTicketDto,
        int changedByUserId);

    Task<bool> TicketExistsAsync(int ticketId);
}
