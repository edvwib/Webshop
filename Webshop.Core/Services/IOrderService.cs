using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Core.Services
{
    public interface IOrderService
    {
        OrderModel GetAddress(string guid);

        bool AddAddress(OrderModel address);

        OrderModel GetOrder(string guid);

        bool AddOrder(string guid);
    }
}
