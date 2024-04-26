using StackExchange.Redis;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Order = Store.Data.Entities.OrderEntities.Order;

namespace Store.Repository.Specification
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string buyerEmail) : 
            base(order=> order.BuyerEmail==buyerEmail)
        {
            AddInclude(order=> order.OrderItems);
            AddInclude(order=> order.DeliveryMethod);
            AddOrderByDescending(order=> order.OrderDate);
        }
        public OrderWithItemsSpecification(Guid id, string buyerEmail) :
            base(order => order.BuyerEmail == buyerEmail&&order.Id==id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
           
        }
    }
}
