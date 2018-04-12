using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Webshop.Core.Models;
using Webshop.Core.Services;
using Webshop.Core.Services.Implementations;
using Webshop.Core.Repositories;
using Webshop.Core.Repositories.Implementations;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        private string _guid;

        public ShopController(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("ConnectionString");

            _productService = new ProductService(new ProductRepository(connectionString));
            _cartService = new CartService(new CartRepository(connectionString), _productService);
            _orderService = new OrderService(new OrderRepository(connectionString), _cartService);
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

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult ConfirmOrder(AddressModel a)
        {
            var guid = GetGuidCookie();

            var address = $"{a.Name} {a.Street}, {a.Zip} {a.City} - {a.Country}";

            _orderService.AddOrder(new OrderModel{
                Guid = guid,
                Email = a.Email,
                Address = address,
                Total = _cartService.GetTotal(guid),
            });

            foreach (var product in _cartService.GetAll(guid))
            {
                _orderService.AddOrderRow(new OrderRowModel
                {
                    Guid = guid,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    ProductCount = product.Count,
                });
            }

            Response.Cookies.Delete("guid");
            _cartService.Empty(guid);

            return View(_orderService.Get(guid));

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
