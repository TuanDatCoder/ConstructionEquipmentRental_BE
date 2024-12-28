using Data.DTOs.Brand;
using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BrandServices
{
    public interface IBrandService
    {
        Task<List<BrandResponseDTO>> GetAllBrandsAsync(int? page, int? size); 
        Task<BrandResponseDTO> GetBrandByIdAsync(int id);
        Task<BrandResponseDTO> CreateBrandAsync(BrandRequestDTO brandRequest);
        Task<BrandResponseDTO> UpdateBrandAsync(int id, BrandRequestDTO brandRequest);
        Task<bool> DeleteBrandAsync(int id);
        Task<BrandResponseDTO> ChangeBrandStatus(int id, BrandStatusEnum newStatus);
    }
}
