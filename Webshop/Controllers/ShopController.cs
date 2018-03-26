using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Repositories.Implementations;
using Webshop.Core.Services.Implementations;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;

        private string _guid;

        public ShopController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionString");
            _productService = new ProductService(new ProductsRepository(connectionString));
            _cartService = new CartService(new CartRepository(connectionString));
        }

        public IActionResult Index()
        {
            return View(_productService.GetAll());
        }

        public IActionResult Product(int id)
        {
            var product = _productService.Get(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult AddToCart(int productId)
        {
            if (!_cartService.Add(GetGuidCookie(), productId))
                return BadRequest();

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            return View(_cartService.GetAll(GetGuidCookie()));
        }

        public IActionResult RemoveItemFromCart(int productId)
        {
            _cartService.Remove(GetGuidCookie(), productId);

            return RedirectToAction("Cart");
        }

        public IActionResult EmptyCart()
        {
            _guid = GetGuidCookie();

            if (_guid == null)
                return RedirectToAction("Cart");

            _cartService.Empty(_guid);
            Response.Cookies.Delete("guid");

            return RedirectToAction("Cart");
        }

        public IActionResult UpdateCartItem(int productId, int count)
        {
            _cartService.UpdateCount(GetGuidCookie(), productId, count);

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutModel address)
        {
            var order = new OrderModel
            {
                Address = address,
                Cart = _cartService.GetAll(GetGuidCookie())
            };

            return View(order);
        }

        private string GetGuidCookie()
        {
            string guidCookie = Request.Cookies["guid"];

            if (guidCookie != null)
                return guidCookie;

            guidCookie = Guid.NewGuid().ToString();
            Response.Cookies.Append("guid", guidCookie);

            return guidCookie;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
