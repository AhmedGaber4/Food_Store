using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Service.Services.Theproducts.Dots;

namespace Store.Service.OrderService.Dto
{
    public class OrderItemResolver : IValueResolver<OrderItem, OrderitemDto, string>
    {

        private readonly IConfiguration _configuration;

        public OrderItemResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderitemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return $"{_configuration["BaseUrl"]} {source.ItemOrdered.PictureUrl}";

            return null;
        }
    }
}