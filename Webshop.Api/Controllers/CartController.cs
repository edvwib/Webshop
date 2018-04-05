using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories;
using Webshop.Core.Repositories.Implementations;
using Webshop.Core.Services;
using Webshop.Core.Services.Implementations;

namespace Webshop.Api.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;

        public CartController(ICartService cartService)
        {
            _cartService = new CartService(_cartRepository, _productService);
        }

        [HttpGet("{guid}")]
        public IEnumerable<CartModel> Get(string guid)
        {
            return _cartService.GetAll(guid);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CartModel addToCart)
        {
            if (_cartService.Add(addToCart.Guid, addToCart.Id))
                return StatusCode(200);

            return StatusCode(500);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] CartModel removeFromCart)
        {
            if (_cartService.Remove(removeFromCart.Guid, removeFromCart.Id))
                return StatusCode(200);

            return StatusCode(500);
        }

        [HttpDelete("{guid}")]
        public IActionResult Delete([FromBody] string guid)
        {
            return StatusCode(501);

            if (_cartService.Empty(guid))
                return StatusCode(200);

            return StatusCode(500);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] CartModel updateCount)
        {
            if (_cartService.UpdateCount(updateCount.Guid, updateCount.Id, updateCount.Count))
                return StatusCode(200);

            return StatusCode(500);
        }
    }
}
