using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Category
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public CategoryStatusEnum Status { get; set; }
        public string? ImageUrl { get; set; }
    }
}
