using System;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketsApp.Application.DTOs.Comments;
using TicketsApp.Application.Interfaces;
using TicketsApp.Domain.Entities;
using TicketsApp.Infrastructure.Data;

namespace TicketsApp.Application.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<CreateCommentDto> _createCommentValidator;

    public CommentService(
        ApplicationDbContext context,
        IValidator<CreateCommentDto> createCommentValidator)
    {
        _context = context;
        _createCommentValidator = createCommentValidator;
    }

    public async Task<TicketCommentDto> AddCommentAsync(
        CreateCommentDto createCommentDto, int userId)
    {
        // Validar
        var validationResult = await _createCommentValidator.ValidateAsync(createCommentDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Verificar que el ticket existe
        var ticketExists = await _context.Tickets.AnyAsync(t => t.TicketId == createCommentDto.TicketId);
        if (!ticketExists)
            throw new InvalidOperationException("Ticket no encontrado");

        var comment = new TicketComment
        {
            TicketId = createCommentDto.TicketId,
            UserId = userId,
            Content = createCommentDto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.TicketComments.Add(comment);

        // Registrar en historial
        var history = new TicketHistory
        {
            TicketId = createCommentDto.TicketId,
            ChangedByUserId = userId,
            FieldChanged = "comment_added",
            OldValue = "",
            NewValue = createCommentDto.Content,
            ChangeType = "COMMENTED",
            CreatedAt = DateTime.UtcNow
        };
        _context.TicketHistories.Add(history);

        await _context.SaveChangesAsync();

        return await GetCommentWithUserAsync(comment.CommentId);
    }

    public async Task<List<TicketCommentDto>> GetTicketCommentsAsync(int ticketId)
    {
        var comments = await _context.TicketComments
            .Include(c => c.User)
            .Where(c => c.TicketId == ticketId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return comments.Adapt<List<TicketCommentDto>>();
    }

    public async Task UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto, int userId)
    {
        var comment = await _context.TicketComments.FirstOrDefaultAsync(c => c.CommentId == commentId);
        if (comment == null)
            throw new InvalidOperationException("Comentario no encontrado");

        if (comment.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para editar este comentario");

        comment.Content = updateCommentDto.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, int userId)
    {
        var comment = await _context.TicketComments.FirstOrDefaultAsync(c => c.CommentId == commentId);
        if (comment == null)
            throw new InvalidOperationException("Comentario no encontrado");

        if (comment.UserId != userId)
            throw new UnauthorizedAccessException("No tienes permiso para eliminar este comentario");

        _context.TicketComments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    private async Task<TicketCommentDto> GetCommentWithUserAsync(int commentId)
    {
        var comment = await _context.TicketComments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CommentId == commentId)
            ?? throw new InvalidOperationException("Comentario no encontrado");

        return comment.Adapt<TicketCommentDto>();
    }
}