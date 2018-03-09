using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        public IActionResult AddToCart(int id)
        {

            var guidCookie = Request.Cookies["guid"];
            if (guidCookie == null)
            {
                _guid = Guid.NewGuid().ToString();
                Response.Cookies.Append("guid", _guid);
            }
            else
            {
                _guid = guidCookie;
            }

            //Check if anything in cart
            CartViewModel cart;
            using (var connection = new SqliteConnection(_connectionString))
            {
                cart = connection.QuerySingleOrDefault<CartViewModel>("SELECT * FROM carts WHERE guid=@guid", new {guid = _guid});
            }

            if (cart == null) //Add new cart to db
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Execute("INSERT INTO carts (guid, productIds) VALUES (@guid, @productIds)", new {guid = _guid, productIds = id});
                }
            }
            else //Update cart
            {
                cart.ProductIds += "," + id;
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Execute("UPDATE carts SET productIds=@productIds WHERE guid=@guid", new {productIds = cart.ProductIds, guid = _guid});
                }
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {

            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
