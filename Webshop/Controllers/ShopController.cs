using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.CodeAnalysis.CSharp;
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
            _guid = GetGuidCookie();

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
            _guid = GetGuidCookie();
            List<ProductViewModel> productsInCart = new List<ProductViewModel>();
            CartViewModel cart;
            List<int> productIds = new List<int>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                cart = connection.QuerySingleOrDefault<CartViewModel>("SELECT * FROM carts WHERE guid=@guid", new {guid = _guid});
            }


            foreach (var productId in cart.ProductIds.Split(','))
            {
                int id;
                if (Int32.TryParse(productId, out id))
                {
                    productIds.Add(id);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }

            foreach (var productId in productIds)
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    productsInCart.Add(connection.QuerySingleOrDefault<ProductViewModel>("SELECT * FROM products WHERE Id=@id", new {id = productId}));
                }
            }

            return View(productsInCart);
        }



        public string GetGuidCookie()
        {
            var guidCookie = Request.Cookies["guid"];

            if (guidCookie != null) return guidCookie;

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
