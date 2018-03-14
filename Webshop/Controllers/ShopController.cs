using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ShopController : Controller
    {
        private readonly string _connectionString;
        private string _guid;

        public ShopController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        public IActionResult Index()
        {
            List<ProductViewModel> products;
            using (var connection = new SqliteConnection(_connectionString))
            {
                products = connection.Query<ProductViewModel>("SELECT * FROM products").ToList();
            }
            return View(products);
        }

        public IActionResult Product(int id)
        {
            ProductViewModel product;
            using (var connection = new SqliteConnection(_connectionString))
            {
                product = connection.QuerySingleOrDefault<ProductViewModel>("SELECT * FROM products WHERE Id=@id", new {id});
            }

            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult AddToCart(int productId)
        {
            _guid = GetGuidCookie();

            using (var connection = new SqliteConnection(_connectionString))
            {

                //Check if item is already in cart, if it is then increment
                var cart = connection.QuerySingleOrDefault("SELECT * FROM carts WHERE guid=@_guid AND productId=@id", new {_guid, id = productId});

                if (cart != null)
                {
                    connection.Execute("UPDATE carts SET count=count+1 WHERE guid=@_guid AND productId=@id", new {_guid, id = productId});
                }
                else
                {
                    connection.Execute("INSERT INTO carts (guid, productId, count) VALUES (@guid, @productId, 1)", new {guid = _guid, productId});
                }
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            _guid = GetGuidCookie();
            List<UserCartViewModel> userCart = GetCart(_guid);


            return View(userCart);
        }

        public IActionResult RemoveItemFromCart(int productId)
        {
            _guid = GetGuidCookie();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute("DELETE FROM carts WHERE productId=@id AND guid=@_guid", new {id = productId, _guid});
            }

            return RedirectToAction("Cart");
        }

        public IActionResult EmptyCart()
        {
            _guid = GetGuidCookie();

            if (_guid == null)
                return RedirectToAction("Cart");

            Response.Cookies.Delete("guid");
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute("DELETE FROM carts WHERE guid=@_guid", new {_guid});
            }

            return RedirectToAction("Cart");
        }

        public IActionResult UpdateCartItem(int productId, int count)
        {
            _guid = GetGuidCookie();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute("UPDATE carts SET count=@count WHERE guid=@_guid AND productId=@productId", new {count, _guid, productId});
            }

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel address)
        {
            return RedirectToAction("ConfirmOrder", address);
        }

        public IActionResult ConfirmOrder(CheckoutViewModel address)
        {
            return View(new OrderViewModel{Address = address, UserCart = GetCart(GetGuidCookie())});
        }

        public string GetGuidCookie()
        {
            var guidCookie = Request.Cookies["guid"];

            if (guidCookie != null)
                return guidCookie;

            guidCookie = Guid.NewGuid().ToString();
            Response.Cookies.Append("guid", guidCookie);

            return guidCookie;
        }

        public List<UserCartViewModel> GetCart(string _guid)
        {
            List<UserCartViewModel> userCart = new List<UserCartViewModel>();
            List<CartViewModel> cart = new List<CartViewModel>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                cart = connection.Query<CartViewModel>("SELECT * FROM carts WHERE guid=@guid", new {guid = _guid}).ToList();
            }

            if (!cart.Any())
                return null;

            foreach (var product in cart)
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    var p = connection.QuerySingleOrDefault<ProductViewModel>("SELECT * FROM products WHERE Id=@id", new {id = product.ProductId});

                    var c = connection.QuerySingleOrDefault<UserCartViewModel>("SELECT * FROM carts WHERE guid=@_guid AND productId=@id", new {_guid, id = product.ProductId});

                    if (p == null)
                        continue;

                    int count = c == null ? 1 : c.Count;

                    userCart.Add(new UserCartViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Count = count
                    });
                }
            }

            return userCart;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
