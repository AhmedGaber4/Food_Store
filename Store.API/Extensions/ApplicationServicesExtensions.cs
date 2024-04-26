using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interface;
using Store.Repository.Repository;
using Store.Service.Services.Theproducts.Dots;
using Store.Service.Services.Theproducts;
using Store.Service.HandleResponses;
using Store.Service.CacheService;
using Store.Repository.BasketRepository;
using Store.Service.Services.BasketService.Dots;
using Store.Service.Services.BasketService;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService;
using Store.Service.OrderService.Dto;
using Store.Service.Services.PaymentService;
using Store.Service.OrderService;

namespace Store.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(model => model.Value.Errors.Count > 0)
                    .SelectMany(model => model.Value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();


                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
