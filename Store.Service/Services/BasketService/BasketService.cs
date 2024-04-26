using AutoMapper;
using Store.Repository.BasketRepository;
using Store.Repository.BasketRepository.Models;
using Store.Service.Services.BasketService.Dots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace Store.Service.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteBasketAsync(string basketid)
          => await _basketRepository.DeleteBasketAsync(basketid);

        public  async Task<CustomerBasketDto> GetBasketAsync(string basketid)
        {
            var basket = await _basketRepository.GetBasketAsync(basketid);
            if (basket is null)
                return new CustomerBasketDto();

            var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);
            
            return mappedBasket;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var customerBasket= _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            var mappedCustpmerBasket = _mapper.Map<CustomerBasketDto>(updatedBasket);

            return mappedCustpmerBasket;
        }
    }
}
