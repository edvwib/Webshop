using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;
using Webshop.Core.Services.Implementations;

namespace Webshop.Api.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionString");
            _cartService = new CartService(new CartRepository(connectionString));
        }

        [HttpGet]
        public IEnumerable<CartModel> Get(string guid)
        {
            return _cartService.GetAll(guid);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CartModel AddToCart)
        {
            if (_cartService.Add(AddToCart.Guid, AddToCart.Id))
                return StatusCode(200);

            return StatusCode(418);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CartModel AddToCart)
        {
            if (_cartService.Add(AddToCart.Guid, AddToCart.Id))
                return StatusCode(200);

            return StatusCode(418);
        }
    }
}
