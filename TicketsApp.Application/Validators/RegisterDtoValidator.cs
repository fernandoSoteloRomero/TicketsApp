using FluentValidation;
using TicketsApp.Application.DTOs.Auth;
using TicketsApp.Application.DTOs.Tickets;
using TicketsApp.Application.DTOs.Comments;
using TicketsApp.Application.DTOs.Users;
using TicketsApp.Application.DTOs.Attachments;
using TicketsApp.Application.DTOs.Deppartments;

namespace TicketsApp.Application.Validators
{
    // ===== AUTH VALIDATORS =====
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email es requerido")
                .EmailAddress().WithMessage("Email debe ser válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña es requerida")
                .MinimumLength(8).WithMessage("Contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]").WithMessage("Contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]").WithMessage("Contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]").WithMessage("Contraseña debe contener al menos un número");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Nombre es requerido")
                .MaximumLength(100).WithMessage("Nombre no puede exceder 100 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Apellido es requerido")
                .MaximumLength(100).WithMessage("Apellido no puede exceder 100 caracteres");
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email es requerido")
                .EmailAddress().WithMessage("Email debe ser válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Contraseña es requerida");
        }
    }

    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken es requerido");
        }
    }

    // ===== TICKET VALIDATORS =====
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Título es requerido")
                .MaximumLength(255).WithMessage("Título no puede exceder 255 caracteres")
                .MinimumLength(5).WithMessage("Título debe tener al menos 5 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descripción es requerida")
                .MinimumLength(10).WithMessage("Descripción debe tener al menos 10 caracteres");

            RuleFor(x => x.AssignedToDepartmentId)
                .GreaterThan(0).WithMessage("Departamento es requerido");

            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("Prioridad es requerida")
                .Must(p => new[] { "BAJA", "MEDIA", "ALTA", "URGENTE" }.Contains(p))
                .WithMessage("Prioridad debe ser BAJA, MEDIA, ALTA o URGENTE");
        }
    }

    public class UpdateTicketDtoValidator : AbstractValidator<UpdateTicketDto>
    {
        public UpdateTicketDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(255).WithMessage("Título no puede exceder 255 caracteres")
                .MinimumLength(5).WithMessage("Título debe tener al menos 5 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Status)
                .Must(s => new[] { "ABIERTO", "EN_PROGRESO", "PENDIENTE_INFO", "RESUELTO", "CERRADO" }.Contains(s))
                .WithMessage("Status inválido")
                .When(x => !string.IsNullOrEmpty(x.Status));

            RuleFor(x => x.Priority)
                .Must(p => new[] { "BAJA", "MEDIA", "ALTA", "URGENTE" }.Contains(p))
                .WithMessage("Prioridad debe ser BAJA, MEDIA, ALTA o URGENTE")
                .When(x => !string.IsNullOrEmpty(x.Priority));
        }
    }

    // ===== COMMENT VALIDATORS =====
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.TicketId)
                .GreaterThan(0).WithMessage("TicketId es requerido");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Contenido del comentario es requerido")
                .MinimumLength(5).WithMessage("Comentario debe tener al menos 5 caracteres")
                .MaximumLength(5000).WithMessage("Comentario no puede exceder 5000 caracteres");
        }
    }

    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Contenido del comentario es requerido")
                .MinimumLength(5).WithMessage("Comentario debe tener al menos 5 caracteres")
                .MaximumLength(5000).WithMessage("Comentario no puede exceder 5000 caracteres");
        }
    }

    // ===== USER VALIDATORS =====
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage("Nombre no puede exceder 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Apellido no puede exceder 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId debe ser válido")
                .When(x => x.DepartmentId.HasValue);
        }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Contraseña actual es requerida");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Nueva contraseña es requerida")
                .MinimumLength(8).WithMessage("Nueva contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]").WithMessage("Contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]").WithMessage("Contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]").WithMessage("Contraseña debe contener al menos un número")
                .NotEqual(x => x.CurrentPassword).WithMessage("La nueva contraseña debe ser diferente a la actual");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");
        }
    }

    public class AssignRoleDtoValidator : AbstractValidator<assignRoleDto>
    {
        public AssignRoleDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId es requerido");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName es requerido")
                .Must(r => new[] { "Empleado", "Agente", "GerenteDpto", "Admin" }.Contains(r))
                .WithMessage("RoleName inválido");
        }
    }

    // ===== DEPARTMENT VALIDATORS =====
    public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nombre del departamento es requerido")
                .MaximumLength(100).WithMessage("Nombre no puede exceder 100 caracteres")
                .MinimumLength(3).WithMessage("Nombre debe tener al menos 3 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descripción es requerida")
                .MaximumLength(255).WithMessage("Descripción no puede exceder 255 caracteres");
        }
    }

    public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Nombre no puede exceder 100 caracteres")
                .MinimumLength(3).WithMessage("Nombre debe tener al menos 3 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Descripción no puede exceder 255 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }

    // ===== ATTACHMENT VALIDATORS =====
    public class CreateAttachmentDtoValidator : AbstractValidator<CreateAttachmentDto>
    {
        public CreateAttachmentDtoValidator()
        {
            RuleFor(x => x.TicketId)
                .GreaterThan(0).WithMessage("TicketId es requerido");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("Nombre del archivo es requerido")
                .MaximumLength(255).WithMessage("Nombre del archivo no puede exceder 255 caracteres");

            RuleFor(x => x.FileUrl)
                .NotEmpty().WithMessage("URL del archivo es requerida");

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("Tamaño del archivo debe ser mayor a 0")
                .LessThanOrEqualTo(5242880).WithMessage("Tamaño del archivo no puede exceder 5MB");
        }
    }
}