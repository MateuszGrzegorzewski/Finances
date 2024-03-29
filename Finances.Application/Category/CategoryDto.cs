﻿namespace Finances.Application.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public string? EncodedName { get; set; }
    }
}