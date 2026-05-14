using Microsoft.AspNetCore.Identity;

namespace TicketsApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public int? DepartmentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Relaciones
        public Department? Department { get; set; }
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
        public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
        public ICollection<TicketHistory> HistoryChanges { get; set; } = new List<TicketHistory>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}