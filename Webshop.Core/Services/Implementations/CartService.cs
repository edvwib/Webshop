using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;

namespace Webshop.Core.Services.Implementations
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly ProductService _productService;

        public CartService(IConfiguration config, CartRepository cartRepository)
        {
            var connectionString = config.GetConnectionString("ConnectionString");

            _cartRepository = cartRepository;
            _productService = new ProductService(new ProductsRepository(connectionString));
        }

        public List<CartModel> GetAll(string guid)
        {
            return _cartRepository.GetAll(guid);
        }

        public CartModel Get(string guid, int productId)
        {
            return _cartRepository.Get(guid, productId);
        }

        public bool Add(string guid, int productId)
        {
            //Check if product exists in database
            if (_productService.Get(productId) == null)
                return false;

            //item already in cart = increment. Else add it
            var inCart = _cartRepository.Get(guid, productId);

            if (inCart == null)
                return _cartRepository.Add(guid, productId);

            return _cartRepository.UpdateCount(guid, productId, ++inCart.Count);
        }

        public bool UpdateCount(string guid, int productId, int count)
        {
            if (count < 0)
                return false;
            if (count == 0)
                return Remove(guid, productId);

            return _cartRepository.UpdateCount(guid, productId, count);
        }

        public bool Remove(string guid, int productId)
        {
            if (productId < 1)
                return false;

            return _cartRepository.Remove(guid, productId);
        }

        public bool Empty(string guid)
        {
            if (guid == null)
                return false;

            return _cartRepository.Empty(guid);
        }
    }
}
