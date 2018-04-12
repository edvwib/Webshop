using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Webshop.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Webshop.Core.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddOrder(OrderModel order)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "INSERT INTO orders " +
                        "(guid, email, address, total) " +
                        "VALUES (@guid, @email, @address, @total)",
                        new {guid = order.Guid, email = order.Email, address = order.Address, total = order.Total});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AddOrderRow(OrderRowModel order)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "INSERT INTO orderRows " +
                        "(guid, productName, productCount, productPrice) " +
                        "VALUES (@guid, @name, @count, @price)",
                        new {guid = order.Guid, name = order.ProductName, count = order.ProductCount, price = order.ProductPrice});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public OrderModel Get(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    return connection.QuerySingleOrDefault<OrderModel>(
                        "SELECT * " +
                        "FROM orders " +
                        "WHERE guid=@guid",
                        new {guid});
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
