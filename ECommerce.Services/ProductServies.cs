using AutoMapper;
using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.Product;
using ECommerce.Services.Exceptions;
using ECommerce.Services.Specifications.ProductSpecifications;
using ECommerce.ServicesAbstraction;
using Shared;
using Shared.CommonResponses;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class ProductServies : IProductServies
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServies(IunitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductBrandDTO>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrandDTO>>(brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var products = await repo.GetAllAsync(spec);

            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var CountOfData = DataToReturn.Count();

            // 
            var CountSpec = new ProductWithCountSpecification(queryParams);
            var CountWithSpec = await repo.CoutnDataAsync(CountSpec);

            return new PaginatedResult<ProductDTO>(
                
                pageIndex : queryParams.PageIndex,
                pageSize : CountOfData,
                totalcount : CountWithSpec,
                data : DataToReturn

            );
        }

        public async Task<IEnumerable<ProductTypeDTO>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductTypeDTO>>(types);
        }

        public async Task<Result<ProductDTO>> GetProductAsync(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);
            var product = await _unitOfWork.GetRepository<Product , int>().GetByIdAsync(spec);

            if (product is null)
                //return Result<ProductDTO>.Fail(Error.NotFound("Product.NotFound", $"Product with id {id} not found"));
            return Error.NotFound("Product.NotFound",$"Product with id {id} not found");

            //return Result<ProductDTO>.Ok(_mapper.Map<ProductDTO>(product));
            return _mapper.Map<ProductDTO>(product);
        }
    }
}
