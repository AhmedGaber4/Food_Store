using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interface;
using Store.Repository.Repository;
using Store.Repository.Specification;
using Store.Service.Helper;
using Store.Service.Services.Theproducts.Dots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Theproducts
{
   
        public class ProductService : IProductService
        {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

            public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
            {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            }
            public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
            {
                var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
                var mappedBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);

                return mappedBrands;
            }

            public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification Input)
            {
            var specs =new ProductsWithSpecifications(Input);

            var Products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecificationsAsync(specs);
            var countSpecs= new ProductsWithFilterAndCountSecifications(Input);
            var count= await _unitOfWork.Repository<Product,int>().CountSpecificationAsyn(countSpecs);
                var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(Products);

                return new PaginatedResultDto<ProductDetailsDto>(Input.PageIndex, Input.PageSize, count, mappedProducts);
            }

            public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
            {
                var Types = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
                var mappedTypes = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(Types);

                return mappedTypes;
            }

            public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
            {

            if (id is null)
                throw new Exception("ID is null");
            var specs= new ProductsWithSpecifications(id);


            var Product = await _unitOfWork.Repository<Product, int>().GetWithSpecificationsByIdAsync(specs);
          
            if (Product is null)
                throw new Exception("Product not found");

                var mappedProduct = _mapper.Map<ProductDetailsDto>(Product);

            return mappedProduct;
            }

        }
    }

