using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;

namespace Webshop.Core.Services.Implementations
{
    public class UserCartService
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public UserCartService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionString");
            _cartService = new CartService(new CartRepository(connectionString));
            _productService = new ProductService(new ProductsRepository(connectionString));
        }

        public List<UserCartModel> GetAll(string guid)
        {
            List<UserCartModel> userCart = new List<UserCartModel>();

            var cart = _cartService.GetAll(guid);

            if (!cart.Any())
                return null;

            foreach (var product in cart)
            {
                var p = _productService.Get(product.ProductId);

                var c = _cartService.Get(guid, product.ProductId);

                if (p == null)
                    continue;

                int count = c == null ? 1 : c.Count;

                userCart.Add(new UserCartModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Count = count
                });
            }

            return userCart;
        }

        public UserCartModel Get(string guid, int productId)
        {
            var cart = _cartService.Get(guid, productId);
            var product = _productService.Get(cart.ProductId);

            if (cart == null || product == null)
                return null;

            return new UserCartModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Count = cart.Count
            };
        }
    }
}
