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


        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
