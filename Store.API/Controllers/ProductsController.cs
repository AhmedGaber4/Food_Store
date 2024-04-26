using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specification;
using Store.Service.HandleResponses;
using Store.Service.Helper;
using Store.Service.Services.Theproducts;
using Store.Service.Services.Theproducts.Dots;

namespace Store.API.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands()
            =>Ok (await _productService.GetAllBrandsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
          => Ok(await _productService.GetAllTypesAsync());

        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification Input)
            => Ok(await _productService.GetAllProductsAsync(Input));
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? id)
        {
            if (id is null)
                return BadRequest(new Response(400,"ID is Null"));
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
                return NotFound(new Response(404));
            return Ok(product);
        }
    }
}
