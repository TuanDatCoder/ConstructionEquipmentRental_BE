using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Brand
{
    public class BrandRequestDTO
    {

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Country { get; set; }


    }
}
