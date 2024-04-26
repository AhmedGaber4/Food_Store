using Store.Service.OrderService.Dto;
using Store.Service.Services.BasketService.Dots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input);
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketID);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentlntentld);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentid);

    }
}
