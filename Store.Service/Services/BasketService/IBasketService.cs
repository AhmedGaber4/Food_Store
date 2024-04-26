using Store.Repository.BasketRepository.Models;
using Store.Service.Services.BasketService.Dots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetBasketAsync(string basketid);

        Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket);
        Task<bool> DeleteBasketAsync(string basketid);
    }
}
