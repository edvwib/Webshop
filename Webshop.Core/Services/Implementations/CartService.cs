using System.Collections.Generic;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;

namespace Webshop.Core.Services.Implementations
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;

        public CartService(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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
            //Check if item is already in cart, if it is then increment
            var cart = _cartRepository.Get(guid, productId);

            if (cart != null)
                return _cartRepository.Update(guid, productId);

            return _cartRepository.Add(guid, productId);
        }

        public bool Update(string guid, int productId)
        {
            return _cartRepository.Update(guid, productId);
        }

        public bool UpdateCount(string guid, int productId, int count)
        {
            return _cartRepository.UpdateCount(guid, productId, count);
        }

        public bool Remove(string guid, int productId)
        {
            return _cartRepository.Remove(guid, productId);
        }

        public bool Empty(string guid)
        {
            return _cartRepository.Empty(guid);
        }
    }
}
