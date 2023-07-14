using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Finances.Domain.Entities;

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