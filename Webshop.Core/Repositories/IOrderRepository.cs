using System.Collections.Generic;
using Webshop.Core.Models;

namespace Webshop.Core.Repositories
{
    public interface IOrderRepository
    {
        bool AddOrder(OrderModel order);

        bool AddOrderRow(OrderRowModel order);

        OrderModel Get(string guid);
    }
}
