﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Finances.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string EncodedName { get; private set; } = default!;

        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
    }
}