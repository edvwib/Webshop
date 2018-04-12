using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Core.Services
{
    public interface IOrderService
    {
        bool AddOrder(OrderModel order);

        bool AddOrderRow(OrderRowModel order);

        OrderModel Get(string guid);
    }
}
