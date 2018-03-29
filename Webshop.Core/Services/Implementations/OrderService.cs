using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;

namespace Webshop.Core.Services.Implementations
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly CartService _cartService;

        public OrderService(IConfiguration config, OrderRepository orderRepository)
        {
            var connectionString = config.GetConnectionString("ConnectionString");

            _orderRepository = orderRepository;
            _cartService = new CartService(config, new CartRepository(connectionString));

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
