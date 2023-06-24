using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Domain.Interfaces;

namespace Finances.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string EncodedName { get; private set; } = default!;

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
    }
}