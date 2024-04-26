using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interface;
using Store.Repository.Repository;
using Store.Repository.Specification;
using Store.Service.OrderService.Dto;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dots;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entities.Product;

namespace Store.Service.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IConfiguration configuration,
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
           _configuration = configuration;
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"] ;

            if (input== null)
                throw new Exception("Basket Is Null");


            var deliveryMethod= await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId.Value);
            var shippingPrice = deliveryMethod.Price;

            foreach (var item in input.BasketItems)
            {
               var product=  await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (item.Price!=product.Price) item.Price = product.Price;
            }

            var service = new PaymentIntentService();
           PaymentIntent paymentIntent ;

            if(string.IsNullOrEmpty(input.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)input.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "usd",

                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await service.CreateAsync(options);
                input.PaymentIntentId = paymentIntent.Id;
                input.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)input.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100)
                 

                };
                await service.UpdateAsync(input.PaymentIntentId, options);
            }
            await _basketService.UpdateBasketAsync(input);
            return input;

        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketID)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];
            var input= await _basketService.GetBasketAsync(basketID);
            if (input == null)
                throw new Exception("Basket Is Null");


            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId.Value);
            var shippingPrice = deliveryMethod.Price;

            foreach (var item in input.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (item.Price != product.Price) item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(input.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)input.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "usd",

                    PaymentMethodTypes = new List<string> { "card" }

                };
                paymentIntent = await service.CreateAsync(options);
                input.PaymentIntentId = paymentIntent.Id;
                input.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)input.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100)


                };
                await service.UpdateAsync(input.PaymentIntentId, options);
            }
            await _basketService.UpdateBasketAsync(input);
            return input;

        }

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentid)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentid);
            var order= await _unitOfWork.Repository<Order,Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception("Order Does Not Exist");

            order.OrderPaymentStatus= OrderPaymentStatus.Failed;

            _unitOfWork.Repository<Order, Guid>().Update(order);
            await _unitOfWork.CompleteAsync() ;

            var mappedOrder = _mapper.Map<OrderResultDto>(order);
         
            return mappedOrder;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentlntentld)
        {

            var specs = new OrderWithPaymentIntentSpecification(paymentlntentld);
            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception("Order Does Not Exist");

            order.OrderPaymentStatus = OrderPaymentStatus.Received;

            _unitOfWork.Repository<Order, Guid>().Update(order);
            await _unitOfWork.CompleteAsync();
            await _basketService.DeleteBasketAsync(order.BasketId);
            var mappedOrder = _mapper.Map<OrderResultDto>(order);
          
            return mappedOrder;
        }
    }
}
