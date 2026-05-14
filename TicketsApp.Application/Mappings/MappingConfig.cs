using Mapster;
using TicketsApp.Domain.Entities;
using TicketsApp.Application.DTOs.Auth;
using TicketsApp.Application.DTOs.Tickets;
using TicketsApp.Application.DTOs.Comments;
using TicketsApp.Application.DTOs.Users;
using TicketsApp.Application.DTOs.Attachments;
using TicketsApp.Application.DTOs.Deppartments;
using TicketsApp.Application.DTOs.History;

namespace TicketsApp.Application.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            //! ===== USER MAPPINGS =====
            TypeAdapterConfig<User, UserDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.Name : null);

            TypeAdapterConfig<User, LoginResponseDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.FirstName, src => src.UserName);

            //! ===== TICKET MAPPINGS =====
            TypeAdapterConfig<Ticket, TicketDto>
                .NewConfig()
                .Map(dest => dest.CreatedByUserName,
                    src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null)
                .Map(dest => dest.AssignedToUserName,
                    src => src.AssignedToUser != null ? src.AssignedToUser.UserName : null)
                .Map(dest => dest.AssignedToDepartmentName,
                    src => src.AssignedToDepartment != null ? src.AssignedToDepartment.Name : null);

            TypeAdapterConfig<Ticket, TicketListDto>
                .NewConfig()
                .Map(dest => dest.AssignedToUserName,
                    src => src.AssignedToUser != null ? src.AssignedToUser.UserName : null)
                .Map(dest => dest.AssignedToDepartmentName,
                    src => src.AssignedToDepartment != null ? src.AssignedToDepartment.Name : null);

            TypeAdapterConfig<CreateTicketDto, Ticket>
                .NewConfig()
                .Map(dest => dest.Status, src => "ABIERTO");

            //! ===== TICKET COMMENT MAPPINGS =====
            TypeAdapterConfig<TicketComment, TicketCommentDto>
                .NewConfig()
                .Map(dest => dest.UserName, src => src.User != null ? src.User.UserName : null);

            //! ===== TICKET ATTACHMENT MAPPINGS =====
            TypeAdapterConfig<TicketAttachment, TicketAttachmentDto>
                .NewConfig()
                .Map(dest => dest.UploadedByUserName,
                    src => src.UploadedByUser != null ? src.UploadedByUser.UserName : null);

            TypeAdapterConfig<CreateAttachmentDto, TicketAttachment>
                .NewConfig();

            //! ===== DEPARTMENT MAPPINGS =====
            TypeAdapterConfig<Department, DepartmentDto>
                .NewConfig();

            TypeAdapterConfig<CreateDepartmentDto, Department>
                .NewConfig();

            //! ===== TICKET HISTORY MAPPINGS =====
            TypeAdapterConfig<TicketHistory, TicketHistoryDto>
                .NewConfig()
                .Map(dest => dest.ChangedByUserName,
                    src => src.ChangedByUser != null ? src.ChangedByUser.UserName : null);
        }
    }
}