namespace Finances.Application.Expense
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Category { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}