using Store.Data.Entities;
using Store.Service.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.OrderService
{
    public interface IOrderService
    {
        Task<OrderResultDto> CreateOrderAsync(OrderDto input);

        Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail) ;


        Task<OrderResultDto> GetOrderByIdAsync(Guid id, string BuyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetAllDeIiveryMethodsAsync();
    }
}
