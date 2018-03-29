using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories.Implementations
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OrderModel GetAddress(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<OrderModel>(
                    "SELECT * " +
                    "FROM addresses " +
                    "WHERE guid=@guid",
                    new {guid});
            }
        }

        public bool AddAddress(OrderModel a)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "INSERT INTO addresses " +
                        "(guid, email, name, street, zip, city, country) " +
                        "VALUES (@guid, @email, @name, @street, @zip, @city, @country)",
                        new {guid = a.Guid, email = a.Email, name = a.Name, street = a.Street, zip = a.Zip, city = a.City, country = a.Country});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public OrderModel GetOrder(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<OrderModel>(
                    "SELECT * " +
                    "FROM orders " +
                    "WHERE guid=@guid",
                    new {guid});
            }
        }

        public bool AddOrder(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "INSERT INTO orders " +
                        "(guid) " +
                        "VALUES (@guid)",
                        new {guid});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
