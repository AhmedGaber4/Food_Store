using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interface;
using Store.Repository.Specification;
using Store.Service.OrderService.Dto;
using Store.Service.Services.BasketService;
using Store.Service.Services.PaymentService;
using Stripe;

using Order = Store.Data.Entities.OrderEntities.Order;
using Product = Store.Data.Entities.Product;

namespace Store.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,
            IBasketService basketService,
            IMapper mapper,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
           _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await _basketService.GetBasketAsync(input.BasketId);
           
            if (basket is null)
                throw new Exception("Basket Not Exist");

            var orderitems = new List<OrderitemDto> ();

            foreach ( var basketitem in basket.BasketItems)
            {
                var productitem = await _unitOfWork.Repository<Product,int>().GetByIdAsync(basketitem.ProductId);

                if (productitem is null) throw new Exception($"product with id {basketitem.ProductId}  Not Exist");
                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId = productitem.Id,
                    ProductName = productitem.Name,
                    PictureUrl = productitem.ImageUrl
                }; var Orderitem = new OrderItem
                {
                    Price = productitem.Price,
                    Quantity=basketitem.Quantity,
                    ItemOrdered= itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderitemDto>(Orderitem);
                
                orderitems.Add(mappedOrderItem);


            }
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if (deliveryMethod is null) throw new Exception("Delivery Method Not Provided");

            var subtotal= orderitems.Sum(item=> item.Quantity * item.Price);

            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (existingOrder != null) 
            {
                _unitOfWork.Repository<Order, Guid>().Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);
            }

            var mappedShippingAddress =_mapper.Map<ShippingAddress>(input.ShippingAddress);

            var mappedOrderItems = _mapper.Map <List<OrderItem>>(orderitems);

            var order = new Order 
            { 
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress=mappedShippingAddress,
                OrderItems=mappedOrderItems,
                BuyerEmail=input.BuyerEmail,
                SubTotal=subtotal,
                BasketId=basket.Id,
                PaymentIntentId=basket.PaymentIntentId
            };
            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var mappedOrder =_mapper.Map<OrderResultDto>(order) ;
          
            return mappedOrder;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeIiveryMethodsAsync()
           => await _unitOfWork.Repository<DeliveryMethod,int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(BuyerEmail);
           
                 var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationsAsync(specs);

            if (orders is { Count: <=0 })
                throw new Exception("you Do Not Have An Orders Yet");
            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);
            return mappedOrders;

        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id,BuyerEmail);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
            if (order is null)
                throw new Exception($"There Is No Order With ld{id}");
            var mappedOrders = _mapper.Map<OrderResultDto>(order);
            return mappedOrders;

        }
    }
}
