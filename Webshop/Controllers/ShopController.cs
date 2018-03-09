﻿using System;
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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute("INSERT INTO carts (guid, productId) VALUES (@guid, @productId)", new {guid = _guid, productId = id});
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            _guid = GetGuidCookie();
            List<ProductViewModel> productsInCart = new List<ProductViewModel>();
            List<CartViewModel> cart = new List<CartViewModel>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                cart = connection.Query<CartViewModel>("SELECT * FROM carts WHERE guid=@guid", new {guid = _guid}).ToList();
            }

            if (!cart.Any())
                return View();

            foreach (var product in cart)
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    productsInCart.Add(connection.QuerySingleOrDefault<ProductViewModel>("SELECT * FROM products WHERE Id=@id", new {id = product.ProductId}));
                }
            }

            return View(productsInCart);
        }

        public IActionResult RemoveItemFromCart()
        {


            return RedirectToAction("Cart");
        }

        public IActionResult EmptyCart()
        {
            if (GetGuidCookie() != null)
            {
                Response.Cookies.Delete("guid");
            }

            return RedirectToAction("Cart");
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
