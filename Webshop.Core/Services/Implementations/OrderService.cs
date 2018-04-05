using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Repositories.Implementations;

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

        public OrderModel GetAddress(string guid)
        {
            return _orderRepository.GetAddress(guid);
        }

        public bool AddAddress(OrderModel address)
        {
            return _orderRepository.AddAddress(address);
        }

        public OrderModel GetOrder(string guid)
        {
            return _orderRepository.GetOrder(guid);
        }

        public bool AddOrder(string guid)
        {
            return _orderRepository.AddOrder(guid);
        }
    }
}
