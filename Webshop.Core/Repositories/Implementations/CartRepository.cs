using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories.Implementations
{
    public class CartRepository
    {
        private readonly string _connectionString;

        public CartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CartModel> GetAll(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.Query<CartModel>(
                    "SELECT c.guid, c.productId, c.count, p.* " +
                    "FROM carts AS c, products AS p " +
                    "WHERE c.guid=@guid AND c.productId=p.id",
                    new {guid}).ToList();
            }
        }

        public CartModel Get(string guid, int productId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<CartModel>(
                    "SELECT c.guid, c.productId, c.count, p.* " +
                    "FROM carts AS c, products AS p " +
                    "WHERE c.productId=@productId " +
                    "AND c.guid=@guid " +
                    "AND c.productId=p.Id",
                    new {guid, productId});
            }
        }

        public bool Add(string guid, int productId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "INSERT INTO carts " +
                        "(guid, productId, count) " +
                        "VALUES (@guid, @productId, 1)",
                        new {guid, productId});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool UpdateCount(string guid, int productId, int count)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "UPDATE carts " +
                        "SET count=@count " +
                        "WHERE guid=@guid " +
                        "AND productId=@productId",
                        new {count, guid, productId});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Remove(string guid, int productId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "DELETE FROM carts " +
                        "WHERE guid=@guid " +
                        "AND productId=@productId",
                        new {guid, productId});
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Empty(string guid)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Execute(
                        "DELETE FROM carts " +
                        "WHERE guid=@guid",
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
