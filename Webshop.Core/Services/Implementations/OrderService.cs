using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Repositories.Implementations;
using System.Collections.Generic;

namespace Webshop.Core.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;

        public OrderService(IOrderRepository orderRepository, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;

        }

        public bool AddOrder(OrderModel order)
        {
            return _orderRepository.AddOrder(order);
        }

        public bool AddOrderRow(OrderRowModel orderRow)
        {
            return _orderRepository.AddOrderRow(orderRow);
        }

        public OrderModel Get(string guid)
        {
            return _orderRepository.Get(guid);
        }
    }
}
