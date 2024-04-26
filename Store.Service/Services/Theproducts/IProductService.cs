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
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);

        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification Input);
        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();

        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();

    }
}
