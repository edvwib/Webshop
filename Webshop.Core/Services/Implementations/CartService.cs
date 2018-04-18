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

        /// <summary>
        /// Returns a list of items in the given cart.
        /// </summary>
        /// <param name="guid">Guid of the cart</param>
        /// <returns>List of cart products</returns>
        public List<CartModel> GetAll(string guid)
        {
            if (!ValidGuid(guid))
                return null;

            return _cartRepository.GetAll(guid);
        }

        /// <summary>
        /// Returns a single product in the given cart.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <param name="productId">Id of a product</param>
        /// <returns>Single cart product</returns>
        public CartModel Get(string guid, int productId)
        {
            if(productId < 1 || !ValidGuid(guid))
                return null;

            return _cartRepository.Get(guid, productId);
        }

        /// <summary>
        /// Returns the total price for the products in the given cart.
        /// If no cart exists with the given guid it returns 0.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <returns>Total price</returns>
        public decimal GetTotal(string guid)
        {
            if (!ValidGuid(guid))
                return 0;

            return _cartRepository.GetTotal(guid);
        }

        /// <summary>
        /// Adds a product to a cart.
        /// Returns true on success. If the product is already in the cart
        /// it will increment it instead.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <param name="productId">Id of a product</param>
        /// <returns>True or false</returns>
        public bool Add(string guid, int productId)
        {
            if(productId < 1 || !ValidGuid(guid))
                return false;

            if (_productService.Get(productId) == null)
                return false;

            var inCart = _cartRepository.Get(guid, productId);

            if (inCart == null)
                return _cartRepository.Add(guid, productId);

            return _cartRepository.UpdateCount(guid, productId, ++inCart.Count);
        }

        /// <summary>
        /// Updates the count of a product in a cart.
        /// Returns true on success. If count is 0 the product will be removed
        /// from the cart.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <param name="productId">Id of a product</param>
        /// <param name="count">The new count</param>
        /// <returns>True or false</returns>
        public bool UpdateCount(string guid, int productId, int count)
        {
            if (count < 0 || productId < 1 || !ValidGuid(guid))
                return false;
            if (count == 0)
                return Remove(guid, productId);

            return _cartRepository.UpdateCount(guid, productId, count);
        }

        /// <summary>
        /// Removes a product from a cart.
        /// Returns true on success.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <param name="productId">Id of a product</param>
        /// <returns>True or false</returns>
        public bool Remove(string guid, int productId)
        {
            if (productId < 1 || !ValidGuid(guid))
                return false;

            return _cartRepository.Remove(guid, productId);
        }

        /// <summary>
        /// Removes all products from a cart.
        /// Returns true on success.
        /// </summary>
        /// <param name="guid">Guid of a cart</param>
        /// <returns>True or false</returns>
        public bool Empty(string guid)
        {
            if (!ValidGuid(guid))
                return false;

            return _cartRepository.Empty(guid);
        }

        /// <summary>
        /// Validates a guid by checking if it is null, whitespace or
        /// not 36 characters.
        /// Returns true on success.
        /// </summary>
        /// <param name="guid">A guid</param>
        /// <returns>True or false</returns>
        public bool ValidGuid(string guid){
            if(string.IsNullOrWhiteSpace(guid) || guid.Length != 36)
                return false;

            return true;
        }
    }
}
