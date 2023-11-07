using Microsoft.AspNetCore.Identity;

namespace Finances.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Category { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }
    }
}