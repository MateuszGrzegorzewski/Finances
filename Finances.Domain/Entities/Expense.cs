using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        public Category Category { get; set; } = default!;
        public int CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}