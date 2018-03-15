using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories.Implementations
{
    public class ProductsRepository
    {
        private readonly string _connectionString;

        public ProductsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductModel> GetAll()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Query<ProductModel>(
                    "SELECT * FROM products").ToList();
            }
        }

        public ProductModel Get(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<ProductModel>(
                    "SELECT * FROM products WHERE Id=@id",
                    new {id});
            }
        }






        public bool Create(ProductModel product)
        {
            return true;
        }

        public bool Edit(ProductModel product)
        {
            return true;
        }
    }
}
