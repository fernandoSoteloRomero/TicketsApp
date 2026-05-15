using System;
using TicketsApp.Application.DTOs.Comments;

namespace TicketsApp.Application.Interfaces;

public interface ICommentService
{
    Task<TicketCommentDto> AddCommentAsync(
        CreateCommentDto createCommentDto, int userId);

    Task<List<TicketCommentDto>> GetTicketCommentsAsync(int ticketId);

    Task UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto,
        int userId);

    Task DeleteCommentAsync(int commentId, int userId);
}
