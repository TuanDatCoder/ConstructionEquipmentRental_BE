using AutoMapper;
using Repositories.ProductImageRepos;
using Repositories.ProductRepos;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IMapper mapper, IProductImageRepository productImageRepository)
        {
            _mapper = mapper;
            _productImageRepository = productImageRepository;
        }


    }
}
