using Data.DTOs.Brand;
using Data.Entities;
using Repositories.BrandRepos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.DTOs.Product;
using Data.Models.Enums;
using Data.Enums;

namespace Services.BrandServices
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<List<BrandResponseDTO>> GetAllBrandsAsync(int? page, int? size)
        {
            var brands = await _brandRepository.GetBrands(page, size);
            return _mapper.Map<List<BrandResponseDTO>>(brands);
        }

        public async Task<BrandResponseDTO> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return brand == null ? null : _mapper.Map<BrandResponseDTO>(brand);
        }

        public async Task<BrandResponseDTO> CreateBrandAsync(BrandRequestDTO request)
        {
            var brand = _mapper.Map<Brand>(request);
            brand.Status = BrandStatusEnum.ACTIVE.ToString();
            await _brandRepository.Add(brand);
            return _mapper.Map<BrandResponseDTO>(brand);
        }

        public async Task<BrandResponseDTO> UpdateBrandAsync(int id, BrandRequestDTO request)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new Exception($"Brand with ID {id} not found.");
            }

            _mapper.Map(request, brand);
            await _brandRepository.Update(brand);
            return _mapper.Map<BrandResponseDTO>(brand);
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new Exception($"Brand with ID {id} not found.");
            }

            await _brandRepository.Delete(brand);
            return true;
        }

        public async Task<BrandResponseDTO> ChangeBrandStatus(int id, BrandStatusEnum newStatus)
        {
            var existing = await _brandRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Brand with ID {id} not found.");
            }

            existing.Status = newStatus.ToString();

            await _brandRepository.Update(existing);

            return _mapper.Map<BrandResponseDTO>(existing);
        }
    }
}
