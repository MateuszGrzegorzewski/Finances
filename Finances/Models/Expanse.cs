using System.ComponentModel.DataAnnotations;

namespace Finances.Models
{
    public class Expanse
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string? Description { get; set; }
    }
}