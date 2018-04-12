using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;
using Webshop.Core.Repositories;

namespace Webshop.Core.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;

        public CartService(ICartRepository cartRepository, IProductService productService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
        }

        public List<CartModel> GetAll(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid) || guid.Length != 36)
                return null;

            return _cartRepository.GetAll(guid);
        }

        public decimal GetTotal(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid) || guid.Length != 36)
                return 0;

            return _cartRepository.GetTotal(guid);
        }

        public CartModel Get(string guid, int productId)
        {
            if(productId <= 0 || guid == null || guid.Length != 36)
                return null;

            return _cartRepository.Get(guid, productId);
        }

        public bool Add(string guid, int productId)
        {
            if(productId <= 0 || string.IsNullOrWhiteSpace(guid) || guid.Length != 36)
                return false;

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
