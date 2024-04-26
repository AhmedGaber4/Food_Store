using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dots;

namespace Store.API.Controllers
{

    public class BasketController : BaseController
    {
        private readonly IBasketService _service;

        public BasketController(IBasketService service)
        {
            _service = service;
        }
        [HttpGet(" {id} ")]

        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
       => Ok(await _service.GetBasketAsync(id));
        [HttpPost]

    public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basket)
        => Ok(await _service.UpdateBasketAsync(basket));
        [HttpDelete]
        public async Task<ActionResult> DeleteBasketAsync(string id)
        => Ok(await _service.DeleteBasketAsync(id));
    }
}
