using System;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Application.DTOs.Tickets;
using TicketsApp.Application.Interfaces;
using TicketsApp.Domain.Entities;
using TicketsApp.Infrastructure.Data;

namespace TicketsApp.Application.Services;

public class TicketService : ITicketService
{
  private readonly ApplicationDbContext _context;
  private readonly IValidator<CreateTicketDto> _createTicketValidator;
  private readonly IValidator<UpdateTicketDto> _updateTicketValidator;

  public TicketService(
    ApplicationDbContext context,
    IValidator<CreateTicketDto> createTicketValidator,
    IValidator<UpdateTicketDto> updateTicketValidator)
  {
    _context = context;
    _createTicketValidator = createTicketValidator;
    _updateTicketValidator = updateTicketValidator;
  }

  public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto, int createdByUserId)
  {
    // Validar
    var validationResult = await _createTicketValidator.ValidateAsync(createTicketDto);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult.Errors);

    // Verificar que el departamento existe
    var departmentExists = await _context.Departments
      .AnyAsync(d => d.DepartmentId == createTicketDto.AssignedToDepartmentId);
    if (!departmentExists)
      throw new InvalidOperationException("Departamento no encontrado");

    // Si hay usuario asignado, verificar que existe y pertenece al departamento
    if (createTicketDto.AssignedToUserId.HasValue)
    {
      var userExists = await _context.Users
        .AnyAsync(u =>
          u.Id == createTicketDto.AssignedToUserId && u.DepartmentId == createTicketDto.AssignedToDepartmentId);
      if (!userExists)
        throw new InvalidOperationException("Usuario no encontrado en ese departamento");
    }

    // Generar número de ticket
    var lastTicket = await _context.Tickets
      .OrderByDescending(t => t.TicketId)
      .FirstOrDefaultAsync();
    var ticketNumber = $"TKT-{(lastTicket?.TicketId ?? 0) + 1:D5}";

    // Crear ticket
    var ticket = createTicketDto.Adapt<Ticket>();
    ticket.TicketNumber = ticketNumber;
    ticket.CreatedByUserId = createdByUserId;
    ticket.Status = "ABIERTO";
    ticket.CreatedAt = DateTime.UtcNow;
    ticket.UpdatedAt = DateTime.UtcNow;

    _context.Tickets.Add(ticket);
    await _context.SaveChangesAsync();

    // Registrar en historial
    var history = new TicketHistory
    {
      TicketId = ticket.TicketId,
      ChangedByUserId = createdByUserId,
      FieldChanged = "ticket_created",
      OldValue = "",
      NewValue = ticketNumber,
      ChangeType = "CREATED",
      CreatedAt = DateTime.UtcNow
    };
    _context.TicketHistories.Add(history);
    await _context.SaveChangesAsync();

    return await GetTicketByIdAsync(ticket.TicketId);
  }

  public async Task<TicketDto> GetTicketByIdAsync(int ticketId)
  {
    var ticket = await _context.Tickets
      .Include(t => t.CreatedByUser)
      .Include(t => t.AssignedToUser)
      .Include(t => t.AssignedToDepartment)
      .Include(t => t.Comments)
      .ThenInclude(c => c.User)
      .Include(t => t.Attachments)
      .ThenInclude(a => a.UploadedByUser)
      .FirstOrDefaultAsync(t => t.TicketId == ticketId);

    if (ticket == null)
      throw new InvalidOperationException("Ticket no encontrado");

    return ticket.Adapt<TicketDto>();
  }

  public async Task<List<TicketListDto>> GetTicketsAsync(int? departmentId = null, string? status = null)
  {
    var query = _context.Tickets
      .Include(t => t.CreatedByUser)
      .Include(t => t.AssignedToUser)
      .Include(t => t.AssignedToDepartment)
      .AsQueryable();

    if (departmentId.HasValue)
      query = query.Where(t => t.AssignedToDepartmentId == departmentId);

    if (!string.IsNullOrEmpty(status))
      query = query.Where(t => t.Status == status);

    var tickets = await query
      .OrderByDescending(t => t.CreatedAt)
      .ToListAsync();

    return tickets.Adapt<List<TicketListDto>>();
  }

  public async Task UpdateTicketAsync(int ticketId, UpdateTicketDto updateTicketDto, int changedByUserId)
  {
    // Validar
    var validationResult = await _updateTicketValidator.ValidateAsync(updateTicketDto);
    if (!validationResult.IsValid)
      throw new ValidationException(validationResult.Errors);

    var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId);
    if (ticket == null)
      throw new InvalidOperationException("Ticket no encontrado");

    // Registrar cambios en historial
    if (!string.IsNullOrEmpty(updateTicketDto.Status) && ticket.Status != updateTicketDto.Status)
    {
      var history = new TicketHistory
      {
        TicketId = ticketId,
        ChangedByUserId = changedByUserId,
        FieldChanged = "status",
        OldValue = ticket.Status,
        NewValue = updateTicketDto.Status,
        ChangeType = "STATUS_CHANGED",
        CreatedAt = DateTime.UtcNow
      };
      _context.TicketHistories.Add(history);

      if (updateTicketDto.Status == "RESUELTO")
        ticket.ResolvedAt = DateTime.UtcNow;
      if (updateTicketDto.Status == "CERRADO")
        ticket.ClosedAt = DateTime.UtcNow;
    }

    if (!string.IsNullOrEmpty(updateTicketDto.Title))
      ticket.Title = updateTicketDto.Title;

    if (!string.IsNullOrEmpty(updateTicketDto.Description))
      ticket.Description = updateTicketDto.Description;

    if (!string.IsNullOrEmpty(updateTicketDto.Priority))
      ticket.Priority = updateTicketDto.Priority;

    if (updateTicketDto.AssignedToUserId.HasValue && ticket.AssignedToUserId != updateTicketDto.AssignedToUserId)
    {
      var history = new TicketHistory
      {
        TicketId = ticketId,
        ChangedByUserId = changedByUserId,
        FieldChanged = "assigned_to_user_id",
        OldValue = ticket.AssignedToUserId?.ToString() ?? "",
        NewValue = updateTicketDto.AssignedToUserId.Value.ToString(),
        ChangeType = "REASSIGNED",
        CreatedAt = DateTime.UtcNow
      };
      _context.TicketHistories.Add(history);

      ticket.AssignedToUserId = updateTicketDto.AssignedToUserId;
    }

    if (updateTicketDto.AssignedToDepartmentId.HasValue &&
        ticket.AssignedToDepartmentId != updateTicketDto.AssignedToDepartmentId)
    {
      var history = new TicketHistory
      {
        TicketId = ticketId,
        ChangedByUserId = changedByUserId,
        FieldChanged = "assigned_to_department_id",
        OldValue = ticket.AssignedToDepartmentId.ToString(),
        NewValue = updateTicketDto.AssignedToDepartmentId.Value.ToString(),
        ChangeType = "REASSIGNED",
        CreatedAt = DateTime.UtcNow
      };
      _context.TicketHistories.Add(history);

      ticket.AssignedToDepartmentId = updateTicketDto.AssignedToDepartmentId.Value;
    }

    ticket.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
  }

  public async Task<bool> TicketExistsAsync(int ticketId)
  {
    return await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);
  }
}
