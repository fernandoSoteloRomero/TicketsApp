using Microsoft.EntityFrameworkCore;
using TicketsApp.Domain.Entities;

namespace TicketsApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    //! DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<TicketHistory> TicketHistories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //! USER CONFIGURATION
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! ROLE CONFIGURATION
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(255);
        });

        //! DEPARTMENT CONFIGURATION
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(255);

            entity.HasIndex(e => e.Name).IsUnique();
        });

        //! TICKET CONFIGURATION
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId);

            entity.Property(e => e.TicketNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("ABIERTO");

            entity.Property(e => e.Priority)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("MEDIA");

            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.AssignedToDepartment)
                .WithMany(d => d.Tickets)
                .HasForeignKey(e => e.AssignedToDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! TICKET COMMENT CONFIGURATION
        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.Property(e => e.Content)
                .IsRequired();

            entity.HasIndex(e => e.TicketId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! TICKET ATTACHMENT CONFIGURATION
        modelBuilder.Entity<TicketAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId);

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.FileUrl)
                .IsRequired();

            entity.HasIndex(e => e.TicketId);

            entity.HasOne(e => e.Ticket)
                .WithMany(t => t.Attachments)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UploadedByUser)
                .WithMany(u => u.Attachments)
                .HasForeignKey(e => e.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! TICKET HISTORY CONFIGURATION
        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId);

            entity.Property(e => e.FieldChanged)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.OldValue)
                .HasMaxLength(500);

            entity.Property(e => e.NewValue)
                .HasMaxLength(500);

            entity.Property(e => e.ChangeType)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.TicketId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.Ticket)
                .WithMany(t => t.History)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChangedByUser)
                .WithMany(u => u.HistoryChanges)
                .HasForeignKey(e => e.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! REFRESH TOKEN CONFIGURATION
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId);

            entity.Property(e => e.Token)
                .IsRequired();

            entity.HasIndex(e => e.UserId);

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        //! AUDIT LOG CONFIGURATION
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditLogId);

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ResourceType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ResourceId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.IpAddress)
                .HasMaxLength(50);

            entity.Property(e => e.UserAgent)
                .HasMaxLength(500);

            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.PerformedByUserId);

            entity.HasOne(e => e.PerformedByUser)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(e => e.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //! Seed de datos iniciales
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        //! Roles
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1, Name = "Empleado", Description = "Usuario regular que crea tickets",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Role
            {
                RoleId = 2, Name = "Agente", Description = "Trabaja en tickets de su departamento",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Role
            {
                RoleId = 3, Name = "GerenteDpto", Description = "Gerente de departamento",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Role
            {
                RoleId = 4, Name = "Admin", Description = "Administrador del sistema",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        //! Departamentos
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DepartmentId = 1, Name = "IT", Description = "Departamento de Tecnología",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Department
            {
                DepartmentId = 2, Name = "RH", Description = "Recursos Humanos",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Department
            {
                DepartmentId = 3, Name = "Finanzas", Description = "Departamento de Finanzas",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Department
            {
                DepartmentId = 4, Name = "Generales", Description = "Solicitudes generales",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
