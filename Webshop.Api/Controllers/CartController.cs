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

        [HttpGet("{guid}")]
        public List<CartModel> GetAll(string guid)
        {
            return _cartService.GetAll(guid);
        }

        [HttpPost("{guid}, {productId}")]
        public HttpStatusCode AddToCart(string guid, int productId)
        {
            if (_cartService.Add(guid, productId))
                return HttpStatusCode.OK;

            return HttpStatusCode.BadRequest;
        }
    }
}
