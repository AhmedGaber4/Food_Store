using AutoMapper;
using Store.Data.Entities.OrderEntities;
using Store.Data.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.OrderService.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderResultDto>().
                ForMember(dest => dest.DeliveryMethodName,option=>option.MapFrom(scr=>scr.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippinPrice,option=>option.MapFrom(scr=>scr.DeliveryMethod.Price));


            CreateMap<OrderItem, OrderitemDto>().
                ForMember(dest => dest.ProductItemId, option => option.MapFrom(scr => scr.ItemOrdered.ProductItemId))
               .ForMember(dest => dest.ProductName, option => option.MapFrom(scr => scr.ItemOrdered.ProductName))
               .ForMember(dest => dest.PictureUrl, option => option.MapFrom(scr => scr.ItemOrdered.PictureUrl))
               .ForMember(dest => dest.PictureUrl, option => option.MapFrom<OrderItemResolver>()).ReverseMap();
        }
    }
}
